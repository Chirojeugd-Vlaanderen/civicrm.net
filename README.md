# Chiro.CiviCrm.Wcf

This project aims to provide a .NET client library for CiviCRM. It connects to the CiviCRM
API using WCF.

## Warning!

### Volatile interface

This is a young project, and its interface (defined in Chiro.CiviCrm.ClientInterfaces) still
changes a lot. And it will keep on changing for a while, at least until issues #22 and #23
are fixed.

### You need to patch CiviCRM

For this to work, you need to patch CiviCRM:
https://github.com/civicrm/civicrm-core/pull/4641/files
(No worries, it is a small patch.)

## How to get the example to work

### API-user

I use CiviCRM on Drupal. Create a Drupal user, which has the following permissions:

* CiviCRM: show all contacts
* CiviCRM: edit all contacts (if you want to be able to do this with the api)
* CiviCRM: create new contacts
* CiviCRM: use CiviCRM
* CiviCRM: use AJAX API

This user is automatically a contact in CiviCRM. You have to assign an API key to the user. This has to be done by a query on the database:

    update civicrm_contact set api_key='mijnkey' where id=12311;

Of course, you replace the 12311 by the real contact-ID of the civi contact, and you better choose something else as api key :-)

### Configuration of Chiro.CiviCrm.Wcf.Example

In `App.config`, you edit the endpoint:

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
