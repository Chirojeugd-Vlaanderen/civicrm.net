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

In `App.config` of Chiro.CiviCrm.Wcf.Example, you edit this line:

    <endpoint 
      address="http://192.168.56.1/dev/sites/all/modules/civicrm/extern/rest.php" 
      binding="webHttpBinding" behaviorConfiguration="civiCrm"
      contract="Chiro.CiviCrm.Api.ICiviCrmApi" />

Replace `http://192.168.56.1/dev` with the url of your Drupal site.

In the Settings of Chiro.CiviCrm.Client, you change the values of `UserKey` and `SiteKey` into the user's API key, and the key of your CiviCrm instance.

In `Program.cs`, replace the value of `contactId` by the ID of an existing contact in your CiviCRM instance.

Make sure that Chiro.CiviCrm.Wcf.Example is the solutions startup project. Now you should be able to run the example.
