ReturnFields
============

Stel dat je de namen nodig hebt van alle leden en leiding van een
bepaalde groep in een bepaald werkjaar. Die leden kun je in 1 keer
opvragen aan de api m.b.v. [chaining](chaining.md), maar omdat een Contact een
nogal uitgebreide entititeit is, zullen er op die manier erg veel
gegevens over de datalijn gaan. Als je enkel de namen nodig hebt, is dat
wat overkill.

Om dit te vermijden, kun je in je request specifieren dat je slechts een
deel van de velden wilt opvragen. Bijvoorbeeld:

```
var request = new ContactRequest
{
ContactType = ContactType.Individual,
// We zijn enkel geinteresseerd in de naam.
// Onderstaande truuk zorgt ervoor dat de rest van de velden
// niet opgeleverd wordt, waardoor er veel minder over de
// lijn zal moeten.
ReturnFields = "first\_name,last\_name"
}
```

Dit request haalt van de eerste 25 contacten enkel de naam en de
voornaam op. De contacten die opgeleverd worden, zullen dus enkel een
naam en voornaam bevatten; de andere velden blijven leeg.

Zie ook dit \[\[Chaining\#Uitgewerkt-voorbeeld|voorbeeldprogramma\]\]
dat een leidingslijst van een bepaalde groep in een bepaald werkjaar
maakt.
