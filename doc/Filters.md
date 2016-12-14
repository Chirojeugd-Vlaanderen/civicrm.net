Filters
=======

Op start- en einddatum van een evenement
----------------------------------------

Oorspronkelijk ondersteunde Chiro.CiviCrm.Wcf enkel vragen voor de API
waarbij gefilterd werd op 'veld x of y moet gelijk zijn aan a of b'.
Andere operatoren dan 'gelijk aan' (zoals bijv. 'kleiner dan', 'like',
...) waren er niet.

Er is intussen een manier om dit wel te doen. En die is op dit moment
geïmplementeerd voor de start- en einddatum van een evenement.

Dit voorbeeld haalt alle bivakken op die bezig zijn op een bepaalde
datum:

```
var request = new EventRequest
{
EventTypeId = (int) EvenementType.Bivak,
StartDate = new Filter&lt;DateTime?&gt; (WhereOperator.Lte, eenDatum),
EndDate = new Filter&lt;DateTime?&gt; (WhereOperator.Gte, eenDatum),
};
```

('Lte' staat voor 'less than or equal'. 'Gte' voor 'greater than or
equal'.)

Op begin- en einddatum van lidrelaties en memberships, of op membership status.
-------------------------------------------------------------------------------

(Let op, dit is in de huidige master branch, maar nog niet in een
'gepackagede' release.)

Dat gaat gelijkaardig als voor bivakken, maar de properties van het
request die je dan moet gebruiken, zijn `StartDateFilter` en
`EndDateFilter`. (`StartDate` en `EndDate` bestaan nog steeds, en
blijven werken zoals vroeger.)

Ik geloof dat Membership ook een property `MembershipStatusIdFilter`
heeft, waarmee je kunt filteren op membership status ID.

Als je op zoek bent naar meer voorbeelden, dan kun je eens kijken in de
unit tests.

Op andere velden
----------------

Je kunt ook op andere velden filteren. In principe op eender welke
property van een request. Maar dat vraagt momenteel nog wat hackwerk.
(Dat wil dus zeggen dat je met de source moet werken, en niet met
geprecompileerde DLL's.)

Wat je doet, is een bestaande property van een request wijzigen. Als die
property van het type `T` is, dan vervang je die door `Filter&lt;T&gt;`.
En dat is alles.

Ik weet niet of het een goed idee is om dat te doen voor alle properties
van alle requests. Misschien wel. Maar het zou de syntax van een
API-call wel wat omslachtiger maken. Enfin. Ik accepteer pull requests
:-)

Uitgewerkt voorbeeld
--------------------

Hier een compleet voorbeeld dat die bivakken ophaalt, samen met hun
adres en organiserende ploeg, en die informatie afdrukt:

Dit zal enkel werken als ieder bivak een adres heeft. Dat heeft te maken
met \#3556. Bij de volgende build na 24 maart zou dat in orde moeten
zijn.

```
using System;
using System.Linq;
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviSandbox.BivakkenBezig.Properties;

namespace Chiro.CiviSandbox.BivakkenBezig
{
class Program
{
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

DateTime eenDatum = new DateTime(2014, 7, 22);

var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

var request = new EventRequest
{
EventTypeId = (int) EvenementType.Bivak,
StartDate = new Filter&lt;DateTime?&gt; (WhereOperator.Lte, eenDatum),
EndDate = new Filter&lt;DateTime?&gt; (WhereOperator.Gte, eenDatum),
LocBlockGetRequest = new LocBlockRequest
{
IdValueExpression = "\$value.loc\_block\_id",
AddressGetRequest = new AddressRequest
{
IdValueExpression = "\$value.address\_id"
},
},
ContactGetRequest = new ContactRequest
{
// Custom field nummers zul je nog altijd moeten opzoeken.
// 56 is voor organiserende ploeg 1.
IdValueExpression = "\$value.custom\_56\_id"
}
};

var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Event&gt;&gt;(
svc =&gt; svc.EventGet(apiKey, siteKey, request));

foreach (var bivak in result.Values)
{
Console.WriteLine("Bivak van {0}, {1} - {2}",
bivak.ContactResult.Values.First().ExternalIdentifier,
bivak.StartDate, bivak.EndDate);
var adres =
bivak.LocBlockResult.Values.First().AddressResult.Values.First();
Console.WriteLine("{0}, {1} {2} - {3}\\n", adres.StreetAddress,
adres.PostalCode, adres.City, adres.Country);
}
Console.WriteLine("Press enter.");
Console.ReadLine();
}
}
}
```
