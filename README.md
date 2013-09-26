# Chiro.CiviCrm.Wcf

**DISCLAIMER**: I am new to CiviCRM.

This project uses WCF to use the CiviCRM API. It is not a full solution, it is just a proof of concept. It contains a small example.

## How to get the example to work

### API-user

I use CiviCRM on Drupal. Create a Drupal user, which has the following permissions:

* CiviCRM: show all contacts
* CiviCRM: edit all contacts (if you want to be able to do this with the api)
* CiviCRM: create new contacts
* CiviCRM: use CiviCRM
* CiviCRM: use AJAX API
* CiviCRM: administer CiviCRM

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

## Shortcomings

This is not the most beautiful solution. Some ugly parts:

### Actual data in the query string (URL)

The most ugly part is in `Chiro.CiviCrm.Api.ICiviCrmApi`:

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate =
            "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create&contact_type={contactType}&contact_id={id}&first_name={firstName}&last_name={lastName}&external_identifier={externalID}&birth_date={birthDate}&deceased_date={deceasedDate}&is_deceased={isDeceased}&gender={gender}&gender_id={genderId}")]
        void ContactSave(string apiKey, string key, int id, string firstName, string lastName, int externalId, ContactType contactType, DateTime birthDate, DateTime deceasedDate, bool isDeceased, Gender gender, int genderId);

I would have preferred to declare it like this:

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate =
            "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create")]
        void ContactSave(string apiKey, string key, Contact contact);

This way, WCF sends the contact info as xml (or json) in the post data of the request. But CiviCRM cannot handle this.

I tried to extend WCF in such a way that it would append the properties of the contact to the URL, but I did not succeed. I guess there might be a way to replace the URI formatter, but I couldn't find out how.

Another solution might be tweaking the CiviCRM API code, to make it read the post data as well.

Now I do the mapping manually in `Chiro.CiviCrm.Client.CiviCrmClient`.

### Chiro.CiviCrm.ServiceContracts.DataContracts.CiviCrmResponse

I created this class because I had troubles deserializing the results of the CiviCRM API. The result of an API request is always put in a node of the type ResultSet, no matter what kind of result it contains, and WCF doesn't seem to like this. I found this workaround on [the Visual Studio forums](http://social.msdn.microsoft.com/Forums/vstudio/en-US/bcd031d7-c8a4-4bb0-8c85-bc5d7b46108a/rest-services-identical-xmlroot-attributes-on-different-classes), but if you know anything about WCF, I would be very grateful if you could verify whether this hack is OK.
