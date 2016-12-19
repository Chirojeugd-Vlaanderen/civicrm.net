HelloWorld
==========

Voor de hello-world-toepassing maak je een nieuwe console applicatie.
Installeer met NuGet Json.Net voor je project. Chiro.CiviCrm.Wcf werd
gebouwd met Json.Net 6.0.7, maar het werkt ook met 6.0.8.

Je project moet refereren naar deze DLL's:

-   Chiro.Cdf.ServiceHelper.dll
-   Chiro.CiviCrm.Api.dll
-   Chiro.CiviCrm.BehaviorExtension.dll

Je kunt die assemblies downloaden uit de 'Files'-sectie van dit project
en van project:ServiceHelper. Of je kunt de sources downloaden, en de
relevante projecten in je solution opnemen. Of je kan de projecten
toevoegen aan je solution als git-submodule.

**Opgelet**: niet elke download uit die 'Files'-sectie bevat alle nodige
en/of juiste dll's (0.2 bevat bijvoorbeeld ServiceContracts in plaats
van BehaviorExtension.) Voor 0.4 is dit opgelost. Joepie.

ServiceHelper is in principe niet nodig; je kunt WCF-services ook op een
andere manier aanspreken. Maar ik gebruik ServiceHelper in de
voorbeelden, omdat we het ook voor GAP op die manier doen.

Creeer settings voor je project, en definieer de settings ApiKey en
SiteKey. ApiKey moet de key krijgen van je API-user, SiteKey is de site
key van de CiviCRM-instantie.

In app.config zet je de WCF-configuratie:
```
&lt;system.serviceModel&gt;
&lt;extensions&gt;
&lt;![](-- The behavior extension for the CiviWebHttpBehavior --&gt;
      &lt;behaviorExtensions&gt;
        &lt;)-- In the line below, the part 'Version=1.0.0.0,
Culture=neutral, PublicKeyToken=null' is NECESSARY --&gt;
&lt;add name="CiviWebHttp"
type="Chiro.CiviCrm.BehaviorExtension.CiviWebHttpBehaviorExtensionElement,
Chiro.CiviCrm.BehaviorExtension, Version=1.0.0.0, Culture=neutral,
PublicKeyToken=null"/&gt;
&lt;/behaviorExtensions&gt;
&lt;/extensions&gt;

&lt;behaviors&gt;
&lt;!-- The endpoint behavior configuration for CiviCRM uses the
CiviWebHttp behavior extension. --&gt;
&lt;endpointBehaviors&gt;
&lt;behavior name="CiviBehaviorConfiguration"&gt;
&lt;CiviWebHttp/&gt;
&lt;/behavior&gt;
&lt;/endpointBehaviors&gt;
&lt;/behaviors&gt;

&lt;bindings&gt;
&lt;![](-- Use a custom content type mapper for the webHttpBinding. This is needed to use Newtonsoft.Json for deserializing the services responses. --&gt;
      &lt;webHttpBinding&gt;
        &lt;binding name="CiviBindingConfiguration" contentTypeMapper="Chiro.CiviCrm.BehaviorExtension.MyRawMapper, Chiro.CiviCrm.BehaviorExtension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"&gt;
          &lt;security mode="Transport" /&gt;
          &lt;)-- Voor http gebruik je security mode None i.p.v.
transport (https).
MAAR DOE GEEN HTTP OVER HET INTERNET MET DIT SYSTEEM! --&gt;
&lt;/binding&gt;
&lt;/webHttpBinding&gt;
&lt;/bindings&gt;

&lt;client&gt;
&lt;!--
Define the endpoint of your API below. You can name the endpoint, if you
want to connect to different CiviCRM instances.
Use the binding and behavior configurations defined above. The service
contract is Chiro.CiviCrm.Api.ICiviCrmApi.
--&gt;
&lt;endpoint
address="https://192.168.124.1/dev2/sites/all/modules/civicrm/extern/rest.php"
binding="webHttpBinding"
bindingConfiguration="CiviBindingConfiguration"
behaviorConfiguration="CiviBehaviorConfiguration"
contract="Chiro.CiviCrm.Api.ICiviCrmApi"/&gt;
&lt;/client&gt;
&lt;/system.serviceModel&gt;
```
Hier pas je het endpoint address aan zodanig dat het naar de API van je
CiviCRM-instantie wijst.

En dan zou dit moeten werken:

```
using System;
using System.Diagnostics;
using System.Linq;
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviSandbox.HelloWorld.Properties;

namespace Chiro.CiviSandbox.HelloWorld
{
class Program
{
static void Main(string\[\] args)
{
string apiKey = Settings.Default.ApiKey;
string siteKey = Settings.Default.SiteKey;

// In het GAP (versie 2.2 alleszins) gebruiken we dependency injection
// om de ChannelProvider te injecteren. Voor dit voorbeeld doen we het
// manueel.
var serviceHelper = new ServiceHelper(new
ChannelFactoryChannelProvider());

var request = new ContactRequest
{
ExternalIdentifier = "60115" // AD-nummer
};
// Dit request zal de contacts ophalen met external identifier gelijk
// aan 60155.

// Roep de service aan, en check of het resultaat OK is.
// (Misschien beter af te handelen in een wrapper class):
var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, request));
if (result.IsError != 0)
{
throw new ApplicationException(result.ErrorMessage);
}

// Doe iets met het resultaat.

Debug.Assert(result.Count == 1);
var contact = result.Values.First();
Console.WriteLine("{0} {1}", contact.FirstName, contact.LastName);

// Van een contact komt (als dat beschikbaar is) 1 adres, 1 e-mailadres
// en 1 telefoonnummer mee.
Console.WriteLine("Adres: {0}, {1} {2} - {3}", contact.StreetAddress,
contact.PostalCode, contact.City,
contact.Country);
Console.WriteLine("E-mail: {0}", contact.Email);
Console.WriteLine("Telefoonnr.: {0}", contact.Phone);

Console.WriteLine("Druk enter.");
Console.ReadLine();
}
}
}
```
