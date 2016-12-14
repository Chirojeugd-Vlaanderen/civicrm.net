Meer voorbeelden
================

Alle ploegen van een gewest ophalen, op basis van stamnummer.
-------------------------------------------------------------

Dit doen we met een slimme chained call.

```
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

var request = new ContactRequest
{
// Haal gewest op, op basis van stamnummer
ExternalIdentifier = "MG /0100",
// Haal via chained call alle groepen van dat gewest op
RelationshipGetRequest = new RelationshipRequest
{
RelationshipTypeId = (int) RelatieType.BovenliggendePloegVan,
// contactID a moet het contactID zijn van de groep in de
// bovenliggende call.
ContactIdAValueExpression = "\$value.id",
// Alleen actieve relaties (dat is meestal een goed idee)
IsActive = true,
// Haal via chained call alle contact-B-groepen op
ContactGetRequest = new ContactRequest
{
IdValueExpression = "\$value.contact\_id\_b"
},
// Alle contacten (ipv standaard 25)
ApiOptions = new ApiOptions { Limit = 0 }
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

// Doorloop alle relaties die we via chained call opgehaald hebben:
foreach (var rel in contact.RelationshipResult.Values)
{
// Druk van iedere relatie de display-name van contact b af.

// Een bug in mijn code zorgt ervoor dat gestopte groepen niet
// worden opgehaald, tenzij je in je request IsDeceased = 1
// zet. https://github.com/Chirojeugd-Vlaanderen/civicrm.net/issues/82
// Ik kijk dus nog eens extra na of er wel degelijk een contact
// meegekomen is met de relatie.
if (rel.ContactResult.Count &gt; 0)
{
var ploeg = rel.ContactResult.Values.First();

Console.WriteLine(ploeg.DisplayName);
}
}

Console.WriteLine("Druk enter.");
Console.ReadLine();
}
```
