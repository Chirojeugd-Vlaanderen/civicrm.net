HttpRequest
===========

Als je in source:Chiro.CiviCrm.Api/ICiviCrmApi.cs kijkt, dan zie je min
of meer hoe Chiro.CiviCrm.Wcf een request doorstuurt naar de API van
CiviCRM: de keys en het request worden gewoon in een url geplakt.

Securitytechnisch is dat niet ideaal. Want zo kun je in de log files van
de server onder andere zien wat de keys zijn die gebruikt worden om de
API aan te spreken. Ik had liever de keys ergens als POST-informatie
doorgegeven, maar dat is niet zo vanzelfsprekend met WCF. Dat verwacht
voor een HTTP-API by default in het POST-gedeelte 1 object, met de
informatie in de properties. In principe kunnen we daarvoor WCF
customiseren. Het zou zelfs geen slechte zaak zijn. Maar niet zo
triviaal, dus voorlopig is dat op de lange baan geschoven ([issue
46](https://github.com/johanv/civicrm.net/issues/46)).

Het voordeel is wel dat dat eenvoudig debugt. Als je nu in de server
logs gaat kijken, kun je de requests gewoon kopieren naar curl.
Bijvoorbeeld:

curl -X POST 'https://server.example.org/lijn-uit-de-log'
