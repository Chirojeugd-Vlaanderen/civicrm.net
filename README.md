# Chiro.CiviCrm.Wcf

This project provides interfaces and a behavior that allow communication with the CiviCRM
API using WCF.

## Warning!

### Volatile interface

This is a young project, and therefore it is still changing a lot. So if you use this
in your own software projects, chances are high that you will have to refactor things
as the master branch moves on.

I will keep you informed when this project becomes more or less stable.

### You need to patch CiviCRM

For all this to work, you need to patch CiviCRM:
https://github.com/civicrm/civicrm-core/pull/4641/files
(No worries, it is a small patch.)

## How to get the examples/unit tests to work

There two example console applications:

* Chiro.CiviCrm.Wcf.Example (various examples)
* Chiro.CiviCrm.Wcf.CustomFieldExample (shows how you can handle custom fields)

There is also a project with unit tests: Chiro.CiviCrm.Wcf.Test.

All those projects are configured in a similar way.
Of course you will not run the examples or the tests on an important CiviCRM instance! :-)

### API-user

I use CiviCRM on Drupal. Create a Drupal user, which has the following permissions:

* CiviCRM: show all contacts
* CiviCRM: edit all contacts
* CiviCRM: create new contacts
* CiviCRM: delete contacts
* CiviCRM: use CiviCRM
* CiviCRM: use AJAX API

This user is automatically a contact in CiviCRM. You have to assign an API key to the user. This has to be done by a query on the database:

    update civicrm_contact set api_key='blablablapi' where id=12311;

Of course, you replace the 12311 by the real contact-ID of the civi contact, and you better choose something else as api key :-)

### Configuration of an example project or the unit test project

In `App.config`, you change the endpoint address to your needs:

      <endpoint 
	address="http://192.168.124.1/dev2/sites/all/modules/civicrm/extern/rest.php" 
	binding="webHttpBinding" bindingConfiguration="MyBindingConfiguration" 
	behaviorConfiguration="civiCrm" contract="Chiro.CiviCrm.Api.ICiviCrmApi"/>

Adapt these lines to configure your CiviCRM site key, and your api user key:

      <setting name="SiteKey" serializeAs="String">
        <value>462e033f1b3495d094f401d89772ba5b</value>
      </setting>
      <setting name="ApiKey" serializeAs="String">
        <value>blablablapi</value>
      </setting>

In `Program.cs`, replace the value of `externalId` by the external ID of an existing contact 
in your CiviCRM instance.

Make sure that Chiro.CiviCrm.Wcf.Example is the solutions startup project. Now you should be able to run the example.
