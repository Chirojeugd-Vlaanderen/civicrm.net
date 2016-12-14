Limit, Offset en Sort
=====================

Als je de API aanroept om bijvoorbeeld alle groepen op te halen:
```
var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, new ContactRequest {
ContactType = ContactType.Organization } ));
```

Dan krijg je niet meteen alle groepen: het resultaat wordt beperkt tot
25 entiteiten. Als je er meer wilt, dan kun je de 'Limit'-optie
gebruiken. Bijvoorbeeld:

```
var request = new ContactRequest
{
ContactType = ContactType.Organization, // Groepen
ApiOptions = new ApiOptions
{
Limit = 50,
Offset = 200,
Sort = "external\_identifier DESC" // StamNr
}
};

var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, request));
```

In dit voorbeeld wordt omgekeerd gesorteerd op stamnummer, worden er 200
resultaten geskipt, en daarna 50 resultaten opgeleverd.

CiviCRM kolomnamen
------------------

Merk op dat de veldnamen in sort de notatie van CiviCRM hebben, en niet
de CamelCase benaming van .NET ([issue
19](https://github.com/johanv/civicrm.net/issues/19")). Vooral voor de
custom fields is de benaming van de kolommen erg verschillend. Je kunt
dit bekijken in

-   source:Chiro.CiviCrm.Api/DataContracts/Requests/RelationshipRequest.CustomFields.cs
-   source:Chiro.CiviCrm.Api/DataContracts/Requests/EventRequest.CustomFields.cs
-   source:Chiro.CiviCrm.Api/DataContracts/Requests/ContactRequest.CustomFields.cs

Maximum lengte van API responses vastleggen
-------------------------------------------

Als je aan je API veel gegevens wil kunnen opvragen (bijvoorbeeld
informatie over 50 groepen), dan moet je in je configuratiefile bij de
binding een attribuut `maxReceivedMessageSize` toevoegen, bijvoorbeeld
`maxReceivedMessageSize="200000"`. Zo niet, krijg je een exception van
WCF als je resultaat de 65535 bytes overschrijdt.

Voorbeeld
---------

```
using System;
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviSandbox.Options.Properties;

namespace Chiro.CiviSandbox.Options
{
class Program
{
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

// Haal alle groepen/ploegen op, sorteer op stamnummer (omgekeerd),
// negeer de eerste 100, en lever de volgende 50 af.
// In de sort-string moet de CiviCRM-naam van de velden gebruikt worden,
en
// niet de CamelCase-namen van de properties in Chiro.CiviCrm.Net.
// https://github.com/johanv/civicrm.net/issues/19
var request = new ContactRequest
{
ContactType = ContactType.Organization, // Groepen
ApiOptions = new ApiOptions
{
Limit = 50,
Offset = 200,
Sort = "external\_identifier DESC" // StamNr
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

// Doe iets met het resultaat.
foreach (var c in result.Values)
{
// De naam van een groep zit in 'OrganizationName'.
Console.WriteLine("{0} {1} {2}", c.ExternalIdentifier,
c.OrganizationName, c.City);
}

Console.WriteLine("Druk enter.");
Console.ReadLine();

}
}
}
```
