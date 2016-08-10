# Chiro.CiviCrm.Wcf

This project provides interfaces and a behavior that allow communication with the CiviCRM
API using WCF.

Please read the extended documentation in [the civicrm.net wiki](https://github.com/Chirojeugd-Vlaanderen/civicrm.net/wiki/).

## How to get the examples/unit tests working

The example program is a console application: Chiro.CiviCrm.Wcf.Example. It contains
various examples.

There is also a project with unit tests: Chiro.CiviCrm.Wcf.Test.

Both projects can be configured in a similar way.
Of course you will not run the examples or the tests on an important CiviCRM instance! :-)

### API-user

I use CiviCRM on Drupal. Create a Drupal user, which has the following permissions:

* CiviCRM: show all contacts
* CiviCRM: edit all contacts
* CiviCRM: create new contacts
* CiviCRM: delete contacts
* CiviCRM: use CiviCRM
* CiviCRM: use AJAX API
* CiviCRM: access all custom data

This user is automatically a contact in CiviCRM. You have to assign an API key to the user. This has to be done by a query on the database:

    update civicrm_contact set api_key='blablablapi' where id=12311;

Of course, you replace the 12311 by the real contact-ID of the civi contact, and you better choose something else as api key :-)

### Configuration of the example project and the unit test project

In `App.config`, you change the endpoint address to your needs:

        <endpoint
          address="http://192.168.124.1/dev2/sites/all/modules/civicrm/extern/rest.php"
          binding="webHttpBinding"
          bindingConfiguration="CiviBindingConfiguration"
          behaviorConfiguration="CiviBehaviorConfiguration"
          contract="Chiro.CiviCrm.Api.ICiviCrmApi"/>

Adapt these lines to configure your CiviCRM site key, and your api user key:

      <setting name="SiteKey" serializeAs="String">
        <value>462e033f1b3495d094f401d89772ba5b</value>
      </setting>
      <setting name="ApiKey" serializeAs="String">
        <value>blablablapi</value>
      </setting>

In `Program.cs`, replace the value of `externalId` by the external ID of an existing contact 
in your CiviCRM instance.

Make sure that Chiro.CiviCrm.Wcf.Example is the solutions startup project. Now you should be able to run the example
by pressing F5. To run the unit tests, press Ctrl-R, followed by A.
