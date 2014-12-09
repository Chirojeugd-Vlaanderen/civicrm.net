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

namespace Chiro.CiviCrm.Api
{
    /// <summary>
    /// WCF service contract for the CiviCRM API
    /// </summary>
    [ServiceContract]
    public interface ICiviCrmApi
    {
        /// <summary>
        /// Find contact with given <paramref name="id"/>
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="id">(CiviCRM) ID of contact to be found</param>
        /// <returns>If found, the (unique) contact with given <paramref name="id"/>,
        /// otherwise <c>null</c>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={id}"
            )]
        CiviContact ContactGet(string apiKey, string key, CiviId id);

        /// <summary>
        /// Find contact with given <paramref name="externalIdentifier"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="externalIdentifier">External identifier of contact to find.</param>
        /// <returns>If found, then (unique) contact with given
        /// <paramref name="externalIdentifier"/>, otherwise <c>null</c>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={externalIdentifier}"
            )]
        CiviContact ContactFind(string apiKey, string key, CiviExternalIdentifier externalIdentifier);

        /// <summary>
        /// Savers or updates the given <paramref name="contact"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="contact">Contact to be saved. If the contact has an ID, the existing contact
        /// will be overwritten. If it hasn't, a new contact is created.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create&sequential=1&json={contact}"
            )]
        CiviResult<CiviContact> ContactSave(string apiKey, string key, CiviContact contact);

        /// <summary>
        /// Returns the adresses of the contact with given <paramref name="contactId"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="contactId">ContactId of the adresses you are looking for.</param>
        /// <returns>A CiviResult containing the adresses of the contact with given <paramref name="ContactId"/>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=get&json={contactId}"
            )]
        CiviResult<CiviAddress> ContactAdressesGet(string apiKey, string key, CiviContactId contactId);
    }
}
