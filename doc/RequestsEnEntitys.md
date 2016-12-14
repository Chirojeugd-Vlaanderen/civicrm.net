Requests en entity's
====================

Chiro.CiviCrm.Wcf ondersteunt in versie 0.1 deze entiteiten:

-   Contact
-   Event
-   Membership
-   Relationship
-   Address
-   Email
-   Im
-   Phone
-   Website

Als je de API wilt aanspreken, dan stuur je een request. Je krijgt dan,
alnaargelang de method die je aanroept, 1 entity, of een resultaat dat
meerdere entity's bevat.

Een request verschilt een beetje van een result, en dat is vooral een
gevolg van hoe CiviCRM omgaat met [Chaining|chained calls](Chaining|chained calls.md), en hoe
resultaten [Filters|gefilterd](Filters|gefilterd.md) kunnen worden. In principe zou er
voor elke entiteit een apart request en een aparte entity class moeten
zijn. ~~In praktijk is dat (nog?) niet zo.~~ Dit is gefixt in de code in
git, maar zit niet in de 0.2 downloads.

De requests zijn te vinden in
source:Chiro.CiviCrm.Api/DataContracts/Requests, en de entity's in
source:Chiro.CiviCrm.Api/DataContracts/Entities. ~~Maar dat is nog niet
in orde voor alle entiteiten. Er zijn er nog een paar die in
source:Chiro.CiviCrm.Api/DataContracts/EntityRequests zitten: entity en
request is in dat geval hetzelfde, maar voor deze entiteiten kan
Chiro.CiviCrm.Wcf nog geen chained calls doorgeven naar de CiviCRM
API.~~

Zoals gezegd: je stuurt een request naar de API, en die API levert dan
een entity of een result dat entity's bevat. Een overzicht van wat
Chiro.CiviCrm.Api momenteel ondersteunt, vind je in
source:Chiro.CiviCrm.Api/ICiviCrmApi.cs.
