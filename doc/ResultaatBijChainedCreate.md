ApiResult bij chained create
============================

Als je 2 create calls [Chaining|chaint](Chaining|chaint.md), dan zit er in het
resultaat enkel een kopie van het 'parent'-object. De gekoppelde andere
entiteiten worden niet opgeleverd; dat is een gevolg van een workaround
voor CiviCRM issue
[CRM-15891](https://issues.civicrm.org/jira/browse/CRM-15891).
