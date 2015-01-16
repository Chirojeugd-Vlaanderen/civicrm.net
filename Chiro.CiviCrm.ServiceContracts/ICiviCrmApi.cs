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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={request}")]
        Contact ContactGetSingle(string apiKey, string key, BaseRequest request);

        /// <summary>
        /// Find one or more contacts.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for contacts to find.</param>
        /// <returns>The requested contacts.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=get&json={request}")]
        ApiResultValues<Contact> ContactGet(string apiKey, string key, BaseRequest request);

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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=delete&json={request}&skip_undelete={skipUndelete}")]
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
        ApiResultValues<Contact> ContactSave(string apiKey, string key, Contact contact);

        /// <summary>
        /// Returns one or more addresses.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the addresses to find.</param>
        /// <returns>The requested addresses</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=get&json={request}")]
        ApiResultValues<Address> AdressesGet(string apiKey, string key, BaseRequest request);

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
        ApiResultValues<Address> AddressSave(string apiKey, string key, Address address);

        /// <summary>
        /// Deletes an address.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the AddressId.</param>
        /// <returns>A CiviResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=delete&json={request}")]
        ApiResult AddressDelete(string apiKey, string key, IdRequest request);

        /// <summary>
        /// Returns one or more phone numbers.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the phone numbers to find.</param>
        /// <returns>The requested phone numbers</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Phone&action=get&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Phone&action=delete&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Email&action=get&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Email&action=delete&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Website&action=get&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Website&action=delete&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Im&action=get&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Im&action=delete&json={request}")]
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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity={entity}&action={action}&json={request}")]
        ApiResult GenericCall(string apiKey, string key, CiviEntity entity, ApiAction action, BaseRequest request);
    }
}
