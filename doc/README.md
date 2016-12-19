Chiro.CiviCrm.Wcf
=================

Dit project laat toe om de project:Chirocivi aan te spreken vanuit een
.NET-toepassing (zoals bijvoorbeeld project:civisync of het
intranet/extranet) via [Windows Communciation
Foundation](http://nl.wikipedia.org/wiki/Windows_Communication_Foundation).

Dit project is voor de Chirospecifieke branch van het upstream project
[op github](https://github.com/johanv/civicrm.net). De chiro-branch
bevat ondersteuning voor de custom fields van de Chirocivi en een aantal
enums voor Chirospecifieke relatietypes, evenementtypes en
lidmaatschapstypes. De master branch zou steeds gelijk moeten lopen met
de upstream master branch.

Algemene issues horen thuis in de [upstream issue
tracker](https://github.com/johanv/civicrm.net/issues). De issue tracker
van dit project is voor Chirospecifieke issues, of issues die aan een
milestone van project:chirocivi gekoppeld moeten worden.

Starten
-------

-   [HelloWorld](HelloWorld.md) - De hello-world-toepassing.
-   [RequestsEnEntitys](RequestsEnEntitys.md) - Requests en entity's.

Technisch
---------

-   [HttpRequest](HttpRequest.md) - Hoe ziet een request naar de CiviCRM-API er
    uit?
-   [ApiExplorer](ApiExplorer.md) - Rechtstreeks opdrachten geven aan de API.
-   [Permissies](Permissies.md) - Permissies van de API user.

Geavanceerd
-----------

-   [Options](Options.md) - Het gebruik van limit, offset en sort.
-   [Chaining](Chaining.md) - Meerdere requests koppelen in één call.
-   [Filters](Filters.md) - Filteren.
-   [ReturnFields](ReturnFields.md) - Controle over de opgeleverde velden.

Aandachtspunten
---------------

-   [GetSingleNietGevonden](GetSingleNietGevonden.md) - Het gedrag van GetSingle als er geen
    resultaat werd gevonden.
-   [ResultaatBijChainedCreate](ResultaatBijChainedCreate.md) - Rariteit in resultaat bij
    chained create.

Voorbeelden
-----------

-   [leiding met afdeling(en) van een groep in een bepaald werkjaar](Chaining.md#uitgewerkt-voorbeeld)
-   [bivakken die op een bepaald moment bezig zijn, met adres](Filters.md#uitgewerkt-voorbeeld)
-   [Alle ploegen van een gewest opvragen](Voorbeelden.md#alle-ploegen-van-een-gewest-ophalen-op-basis-van-stamnummer)
-   **TODO:** Leding van een ploeg samen met de cursussen die ze volgden (geblokkeerd door #62)

