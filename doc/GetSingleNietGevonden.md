GetSingle en niet-bestaande resultaten
======================================

De CiviCRM-API biedt voor de meeste entiteiten een 'get'-actie aan, en
een 'getsingle'-actie. De get-actie op Contact wordt gedemonstreerd in
het HelloWorld-project:

```
var result =
serviceHelper.CallService&lt;ICiviCrmApi,
ApiResultValues&lt;Contact&gt;&gt;(
svc =&gt; svc.ContactGet(apiKey, siteKey, request));
```

Dit levert iets op van het type `ApiResultValues&lt;Contact&gt;`, waarin
je dan vindt of de operatie gelukt is (`IsError`), een eventueel
foutbericht (`ErrorMessage`), en dan een array van contactgegevens in
`Values`, die dan alle gevonden items bevat.

Als je er zeker van bent dat je vraag maar 1 resultaat gaat opleveren,
kun je 'getsingle' gebruiken in plaats van 'get':

```
var result =
serviceHelper.CallService&lt;ICiviCrmApi, Contact&gt;(
svc =&gt; svc.ContactGetSingle(apiKey, siteKey, request));
```

Hier is het resultaat meteen een contact. Maar als er niemand is
gevonden, of meer dan 1 contact, dan gaat je resultaat een leeg contact
(`new Contact()`) zijn, en niet `null` zoals je misschien zou
verwachten. Dat komt omdat WCF het foutbericht van de API zal willen
deserializeren naar een `Contact`, en als dat niet lukt, krijg je er dus
een leeg. Controleer dus na een dergelijke call of het `Id` van het
contact groter is dan `0`, in dat geval ben je zeker dat je een zinvol
resultaat hebt.

Je zou er ook voor kunnen kiezen om geen getsingle te gebruiken, enkel
get. Daar wordt wél een lege array opgeleverd als er geen resultaat is
gevonden.
