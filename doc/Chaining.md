Chaining
========

Eenvoudige koppeling
--------------------

Stel dat je een persoon wilt opvragen met al zijn adressen. Dan kun je
dat met een chained call. Bijvoorbeeld:

```
var request = new ContactRequest
{
ExternalIdentifier = "60115", // AD-nummer
AddressGetRequest = new AddressRequest(),
};
```

Als je dit request naar `ContactGetSingle` stuurt, dan krijg je het
contact met gegeven AD-nummer, en in de `AddressResult`-property het
resultaat van een API-call die alle adressen opvraagt met als
`contactId` het opgevraagde contact. Omdat het addressrequest 'kind' is
van een contactrequest, weet CiviCRM dat het het adres aan het contact
moet koppelen via het `ContactId`. Ik heb de indruk dat deze 'magie'
enkel werkt voor zaken die je aan een contact koppelt. Voor andere
entiteiten moet je de koppeling expliciet meegeven, daarover verder
meer.

```
using System;
using System.Linq;
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviSandbox.Chaining.Properties;

namespace Chiro.CiviSandbox.Chaining
{
class Program
{
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

var request = new ContactRequest
{
ExternalIdentifier = "60115", // AD-nummer
AddressGetRequest = new BaseRequest(),
};

var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, request));
if (result.IsError != 0)
{
throw new ApplicationException(result.ErrorMessage);
}

// Doe iets met het resultaat
var contact = result.Values.First();

// Doe iets met het resultaat.
foreach (var a in contact.AddressResult.Values)
{
// De naam van een groep zit in 'OrganizationName'.
Console.WriteLine("{0} {1} {2}", a.StreetAddress, a.PostalCode,
a.City);
}

Console.WriteLine("Druk enter.");
Console.ReadLine();
}
}
}
```

Specifiekere chained calls
--------------------------

Als je een chained call naar de API stuurt, dan stuur je eigenlijk twee
of meer requests. In het voorbeeld hierboven was het tweede request
leeg, maar je zou dat ook specifieker kunnen doen. Je zou bijvoorbeeld
een contact kunnen ophalen met al zijn Dubbelpuntabonnementen:

```
var request = new ContactRequest
{
ExternalIdentifier = "60115", // AD-nummer
MembershipGetRequest = new MembershipRequest
{
MembershipTypeId = (int)MembershipType.DubbelpuntAbonnement
}
};
```

Dit specifiek filteren werkt enkel als het 'geneste' request een 'echt
request' is (contact, membership, event, relationship), en geen
'EntityRequest' (op dit moment onder meer e-mail en phone). (Zie ook
[RequestsEnEntitys](RequestsEnEntitys.md).)

\$value
-------

Als je een entiteit ophaalt die geen contact is, en je wil aan die
API-call een andere chainen, dan is CiviCRM vaak niet slim genoeg om
zelf te weten hoe hij de entiteiten moet koppelen. In dat geval moet je
dit expliciet meegeven. Bijvoorbeeld als je een event wil ophalen samen
met zijn plaats.

Dat zit trouwens in CiviCRM sowieso een beetje vreemd in elkaar. Een
evenement heeft een 'LocBlock', en aan zo'n LocBlock kun je 0 tot 2
adressen koppelen. Met dit request haal je een evenement op samen met
zijn locblock en eerste adres.

```
var request = new EventRequest
{
Id = 1, // een ID.
LocBlockGetRequest = new LocBlockRequest
{
IdValueExpression = "\$value.loc\_block\_id",
AddressGetRequest = new AddressRequest
{
IdValueExpression = "\$value.address\_id"
},
},
};
```

De constructie met `IdValueExpression` zorgt ervoor dat de waarde
`$value.loc_block_id` aan de API wordt doorgegeven als locblock-ID, en
de waarde `$value.address_id` als adres-ID. Op die manier weet de API
dat hij moet zoeken naar een LocBlock waarvan het ID gelijk is aan het
LocBlockId van het evenment, en een adres met het AdresId van het
locblock.

Dit is duidelijk een hack, maar het was een bruikbare manier om die
string door te geven als `Id`, hoewel het `Id` van een LockBlock of
Address een integer moet zijn.

**Let op:** Als het gevraagde evenement geen adres heeft, dan throwt de
API een exception. Dit is mogelijk een bug in de API, dat moet ik nog
eens uitzoeken.

Relationships
-------------

Als je op dezelfde manier een contact wilt opvragen samen met zijn
relaties van bijvoorbeeld een bepaald type, dan is het een beetje
moeilijker. De automatische koppeling via `ContactId` werkt daar niet,
omdat een relationship 2 contact-ID's heeft, en die heten `ContactIdA`
en `ContactIdB`. Om vanuit een `ContactRequest` een chained call naar
Relationship te maken, moet je expliciet meegeven hoe die koppeling moet
gebeuren.

Bijvoorbeeld, een groep ophalen samen met zijn leiding van een bepaald
werkjaar, lukt met dit request:

```
var request = new ContactRequest
{
ExternalIdentifier = "MG /0113", // StamNummer
RelationshipGetRequest = new RelationshipRequest
{
RelationshipTypeId = (int)RelatieType.LidVan,
Afdeling = Afdeling.Leiding,
// Filteren op end date is het makkelijkst om de leiding van een
// bepaald werkjaar op te halen:
EndDate = new DateTime(2005,8,31),
ContactIdBValueExpression = "\$value.id",
}
};
```

De truuk zit 'em in `ContactIdBValueExpression = "$value.id"`. Dit wordt
geinterpreteerd als `ContactIdB` van de relatie moet gelijk zijn aan het
id van de parent (in dit geval Contact).

Dit is niet direct de mooiste hack. Maar op dit moment is het wel de
manier om relaties aan contacten te koppelen. `id` is lowercase; je moet
voor deze hack de \[\[Options\#CiviCRM-kolomnamen|CiviCRM-kolomnamen\]\]
van de velden gebruiken.

Om samen met een relatie een contact op te halen, gebruik je dezelfde
truuk, maar dan omgekeerd:

```
var request = new RelationshipRequest
{
// Een ID van een relationship:
Id = 12345,
ContactGetRequest = new ContactRequest
{
IdValueExpression = "\$value.contact\_id\_a",
}
}
```

### Uitgewerkt voorbeeld

In dit volledig voorbeeld wordt van een bepaalde groep de leidingsploeg
opgehaald uit een gegeven werkjaar. Van iedereen in de leiding wordt
voornaam, naam en afdelingen getoond.

```
using System;
using System.Linq;
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviSandbox.LeidingLijst.Properties;

namespace Chiro.CiviSandbox.LeidingLijst
{
class Program
{
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

var request = new ContactRequest
{
ExternalIdentifier = "MG /0113", // StamNummer
RelationshipGetRequest = new RelationshipRequest
{
RelationshipTypeId = (int)RelatieType.LidVan,
Afdeling = Afdeling.Leiding,
// Filteren op end date is het makkelijkst om de leiding van een
// bepaald werkjaar op te halen:
EndDate = new DateTime(2005,8,31),
ContactIdBValueExpression = "\$value.id",
ContactGetRequest = new ContactRequest
{
IdValueExpression = "\$value.contact\_id\_a",
// We zijn enkel geinteresseerd in de naam van de leiding.
// Onderstaande truuk zorgt ervoor dat de rest van de velden
// niet opgeleverd wordt, waardoor er veel minder over de
// lijn zal moeten.
ReturnFields = "first\_name,last\_name"
}
}
};

var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, request));
if (result.IsError != 0)
{
throw new ApplicationException(result.ErrorMessage);
}

// Doe iets met het resultaat
var contact = result.Values.First();

foreach (var rel in contact.RelationshipResult.Values)
{
// We weten dat het er maar 1 zal zijn:
var leiding = rel.ContactResult.Values.First();

Console.WriteLine("{0} {1} - afdeling(en): {2}", leiding.FirstName,
leiding.LastName,
String.Join(",", rel.LeidingVan.Select(afd =&gt; afd.ToString())));
}

Console.WriteLine("Druk enter.");
Console.ReadLine();
}
}
}
```
