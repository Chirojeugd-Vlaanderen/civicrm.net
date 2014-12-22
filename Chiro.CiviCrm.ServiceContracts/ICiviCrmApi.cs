/*
   Copyright 2013, 2014 Chirojeugd-Vlaanderen vzw

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
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={request}")]
        ApiResultValue<Contact> ContactGet(string apiKey, string key, BaseRequest request);

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
        ApiResultValue<Contact> ContactSave(string apiKey, string key, Contact contact);

        /// <summary>
        /// Returns one or more addresses.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Selection criteria for the addresses to find.</param>
        /// <returns>The requested </returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=get&json={request}")]
        ApiResultValue<Address> AdressesGet(string apiKey, string key, BaseRequest request);

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
        ApiResultValue<Address> AddressSave(string apiKey, string key, Address address);

        /// <summary>
        /// Deletes an address with given <paramref name="addressId"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="request">Request containing the AddressId.</param>
        /// <returns>A CiviResult</returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=delete&json={request}")]
        ApiResult AddressDelete(string apiKey, string key, IdRequest request);
    }
}
