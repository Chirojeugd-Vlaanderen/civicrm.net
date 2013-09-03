# Chiro.CiviCrm.Wcf

**DISCLAIMER**: I am new to CiviCRM, and I am not sure about the way I call the web service.

This project uses WCF to use the CiviCRM API. It contains a small example.

## How to get the example to work

### API-user

I use CiviCRM on Drupal. Create a Drupal user, which has the following permissions:

* CiviCRM: show all contacts
* CiviCRM: use CiviCRM
* CiviCRM: use AJAX API
* CiviCRM: administer CiviCRM

This user is automatically a contact in CiviCRM. You have to assign an API key to the user. This has to be done by a query on the database:

    update civicrm_contact set api_key='mijnkey' where id=12311;

Of course, you replace the 12311 by the real contact-ID of the civi contact, and you better choose something else as api key :-)

### Configuration of Chiro.CiviCrm.Wcf.Example

In `App.config` of Chiro.CiviCrm.Wcf.Example, you edit this line:

    <endpoint 
      address="http://192.168.2.55/dev/sites/all/modules/civicrm/extern/rest.php" 
      binding="webHttpBinding" behaviorConfiguration="civiCrm"
      contract="Chiro.CiviCrm.ServiceContracts.ICiviCrmApi" />

Replace `http://192.168.2.55/dev` with the url of your Drupal site.

In the Settings of Chiro.CiviCrm.Wcf.Example, you change the values of `UserKey` and `SiteKey` into the user's API key, and the key of your CiviCrm instance.

In `Program.cs`, replace the value of `externalID` by an existing external ID of your civicrm instance.

Now you should be able to run the example.