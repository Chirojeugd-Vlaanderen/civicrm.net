/*
   Copyright 2013-2015 Chirojeugd-Vlaanderen vzw

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;
using Chiro.CiviCrm.Api.DataContracts.Requests;

namespace Chiro.CiviCrm.Api
{
    /// <summary>
    /// WCF service contract for the CiviCRM API
    /// </summary>
    [ServiceContract]
    public interface ICiviCrmApi: IDisposable
    {       
        /// <summary>
        /// Find a single contact based on the <paramref name="request"/>
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">search criteria for the contact</param>
        /// <returns>If found, the unique contact statisfying the <paramref name="request"/>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={request}&sequential=1")]
        Contact ContactGetSingle(string apiKey, string key, ContactRequest request);

        /// <summary>
        /// Find one or more contacts.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for contacts to find.</param>
        /// <returns>The requested contacts.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=get&json={request}&sequential=1")]
        ApiResultValues<Contact> ContactGet(string apiKey, string key, ContactRequest request);

        /// <summary>
        /// Deletes a contact.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the ContactId</param>
        /// <param name="skipUndelete">If 0, set the 'is_deleted' flag. If 1, effectively delete the record.</param>
        /// <returns>A CiviResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=delete&json={request}&skip_undelete={skipUndelete}&sequential=1")]
        ApiResult ContactDelete(string apiKey, string key, IdRequest request, int skipUndelete);

        /// <summary>
        /// Saves or updates the given <paramref name="contact"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="contact">Contact to be saved. If the contact has an ID, the existing contact
        /// will be overwritten. If it hasn't, a new contact is created.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create&sequential=1&json={contact}")]
        ApiResultValues<Contact> ContactSave(string apiKey, string key, ContactRequest contact);

        /// <summary>
        /// Saves or updates the given <paramref name="contact"/>. This call works around upstream
        /// issue CRM-15815 by omitting sequential=1. Which causes the result to be unparsable.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="contact">Contact to be saved. If the contact has an ID, the existing contact
        /// will be overwritten. If it hasn't, a new contact is created.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create&json={contact}")]
        EmptyResult ContactSaveWorkaroundCrm15815(string apiKey, string key, ContactRequest contact);

        /// <summary>
        /// Returns one or more addresses.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the addresses to find.</param>
        /// <returns>The requested addresses</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=get&json={request}&sequential=1")]
        ApiResultValues<Address> AdressGet(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="address"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="address">Address to be saved</param>
        /// <returns>A CiviResult containing the saved address.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=create&sequential=1&json={address}")]
        ApiResultValues<Address> AddressSave(string apiKey, string key, AddressRequest address);

        /// <summary>
        /// Deletes an address.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the AddressId.</param>
        /// <returns>A CiviResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=delete&json={request}&sequential=1")]
        ApiResult AddressDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more relationships.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the relationships to find.</param>
        /// <returns>The requested relationships</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Relationship&action=get&json={request}&sequential=1")]
        ApiResultValues<Relationship> RelationshipGet(string apiKey, string key, RelationshipRequest request);

        /// <summary>
        /// Returns one relationship.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the relationships to find.</param>
        /// <returns>The requested relationship</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Relationship&action=getsingle&json={request}&sequential=1")]
        Relationship RelationshipGetSingle(string apiKey, string key, RelationshipRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="relationship"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="relationship">Relationship to be saved</param>
        /// <returns>A CiviResult containing the saved relationship.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Relationship&action=create&sequential=1&json={relationship}")]
        ApiResultValues<Relationship> RelationshipSave(string apiKey, string key, RelationshipRequest relationship);

        /// <summary>
        /// Deletes a relationship.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the RelationshipId.</param>
        /// <returns>A CiviResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Relationship&action=delete&json={request}&sequential=1")]
        ApiResult RelationshipDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more phone numbers.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the phone numbers to find.</param>
        /// <returns>The requested phone numbers</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Phone&action=get&json={request}&sequential=1")]
        ApiResultValues<Phone> PhoneGet(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="phoneNumber"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="phoneNumber">Phone number to be saved</param>
        /// <returns>A CiviResult containing the saved phone number.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Phone&action=create&sequential=1&json={phoneNumber}")]
        ApiResultValues<Phone> PhoneSave(string apiKey, string key, Phone phoneNumber);

        /// <summary>
        /// Deletes a phone number.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the PhoneId.</param>
        /// <returns>An ApiResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Phone&action=delete&json={request}&sequential=1")]
        ApiResult PhoneDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more e-mail addresses.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the e-mail addresses to find.</param>
        /// <returns>The requested e-mail addresses</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Email&action=get&json={request}&sequential=1")]
        ApiResultValues<Email> EmailGet(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="emailAddress"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="emailAddress">E-mail address to be saved</param>
        /// <returns>A CiviResult containing the saved e-mail address.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Email&action=create&sequential=1&json={emailAddress}")]
        ApiResultValues<Email> EmailSave(string apiKey, string key, Email emailAddress);

        /// <summary>
        /// Deletes an e-mail address.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the MailId.</param>
        /// <returns>An ApiResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Email&action=delete&json={request}&sequential=1")]
        ApiResult EmailDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more websites.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the websites to find.</param>
        /// <returns>The requested websites</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Website&action=get&json={request}&sequential=1")]
        ApiResultValues<Website> WebsiteGet(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="website"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="website">Website address to be saved</param>
        /// <returns>A CiviResult containing the saved website.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Website&action=create&sequential=1&json={website}")]
        ApiResultValues<Website> WebsiteSave(string apiKey, string key, Website website);

        /// <summary>
        /// Deletes a website.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the WebsiteId.</param>
        /// <returns>An ApiResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Website&action=delete&json={request}&sequential=1")]
        ApiResult WebsiteDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more IM.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the IM to find.</param>
        /// <returns>The requested IM</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Im&action=get&json={request}&sequential=1")]
        ApiResultValues<Im> ImGet(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Creates or updates the given <paramref name="im"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="im">Im to be saved</param>
        /// <returns>A CiviResult containing the saved im.</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Im&action=create&sequential=1&json={im}")]
        ApiResultValues<Im> ImSave(string apiKey, string key, Im im);

        /// <summary>
        /// Deletes an Im.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the ImId.</param>
        /// <returns>An ApiResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Im&action=delete&json={request}&sequential=1")]
        ApiResult ImDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Performs a generic call to the API. Please avoid this :-)
        /// </summary>
        /// <param name="apiKey">API-key of your API-user.</param>
        /// <param name="key">Site key of your CiviCRM-instance.</param>
        /// <param name="entity">Entity type for request.</param>
        /// <param name="action">Action on the entity type.</param>
        /// <param name="request">Generic request</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity={entity}&action={action}&json={request}&sequential=1")]
        ApiResult GenericCall(string apiKey, string key, CiviEntity entity, ApiAction action, BaseRequest request);

        /// <summary>
        /// Generic getsingle-action. Returns a JObject, which you'll have to cast to a CiviCRM object
        /// afterwards.
        /// </summary>
        /// <example>
        /// Contact result = (client.GetSingle(apiKey, key, CiviEntity.Contact, new IdRequest(2)) as JObject).ToObject&lt;Contact&gt;
        /// </example>
        /// <param name="apiKey">API-key of your API-user.</param>
        /// <param name="key">Site key of your CiviCRM-instance.</param>
        /// <param name="entity">Entity type for request.</param>
        /// <param name="request">Generic request</param>
        /// <param name="result">This will contain the result.</param>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity={entity}&action=getsingle&json={request}&sequential=1")]
        Object GetSingle(string apiKey, string key, CiviEntity entity, BaseRequest request);

        /// <summary>
        /// Find one or more memberships.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for memberships to find.</param>
        /// <returns>An API-result containing the requested memberships.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Membership&action=get&json={request}&sequential=1")]
        ApiResultValues<Membership> MembershipGet(string apiKey, string key, MembershipRequest request);

        /// <summary>
        /// Deletes a membership.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the MembershipId</param>
        /// <returns>An API-result</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Membership&action=delete&json={request}&sequential=1")]
        ApiResult MembershipDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Saves or updates the membership defined by <paramref name="request"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Membership to be saved. If the membership has an ID, the existing contact
        /// will be overwritten. If it hasn't, a new contact is created.</param>
        /// <returns>An API-result</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Membership&action=create&sequential=1&json={request}")]
        ApiResultValues<Membership> MembershipSave(string apiKey, string key, MembershipRequest request);

        /// <summary>
        /// Find one or more events.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for events to find.</param>
        /// <returns>An API-result containing the requested events.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Event&action=get&json={request}&sequential=1")]
        ApiResultValues<Event> EventGet(string apiKey, string key, EventRequest request);

        /// <summary>
        /// Find one event.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the event to find.</param>
        /// <returns>The requested event.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Event&action=getsingle&json={request}&sequential=1")]
        Event EventGetSingle(string apiKey, string key, EventRequest request);

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the EventId</param>
        /// <returns>An API-result</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Event&action=delete&json={request}&sequential=1")]
        ApiResult EventDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Saves or updates the event defined by <paramref name="request"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Event to be saved. If the event has an ID, the existing event
        /// will be overwritten. If it hasn't, a new event is created.</param>
        /// <returns>An API-result</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Event&action=create&sequential=1&json={request}")]
        ApiResultValues<Event> EventSave(string apiKey, string key, EventRequest request);
    }
}
