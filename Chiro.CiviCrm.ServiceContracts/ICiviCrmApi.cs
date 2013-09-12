/*
   Copyright 2013 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.ServiceContracts.DataContracts;

namespace Chiro.CiviCrm.ServiceContracts
{
    /// <summary>
    /// WCF service contract for the CiviCRM API
    /// </summary>
    [ServiceContract]
    [XmlSerializerFormat]
    public interface ICiviCrmApi: IDisposable
    {
        /// <summary>
        /// Find contact with given <paramref name="externalId"/>
        /// </summary>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="externalId">External ID of contact to be found</param>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <returns>If found, a set with the (unique) contact with 
        /// given <paramref name="externalId"/>,
        /// otherwise <c>null</c>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=get&external_identifier={externalId}"
            )]
        ContactSet ContactFind(string apiKey, string key, int externalId);

        /// <summary>
        /// Saves a new CiviCRM contact, or updates an existing CiviCRM contact.
        /// </summary>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <param name="key">Key of  the CiviCRM-installation</param>
        /// <param name="id">ID of the contact to be updated, or 0 for a new contact</param>
        /// <param name="firstName">new first name</param>
        /// <param name="lastName">new last name</param>
        /// <param name="externalId">new external id</param>
        /// <param name="contactType">new ContacType</param>
        /// <param name="birthDate">date of birth</param>
        /// <param name="deceasedDate">date of death</param>
        /// <param name="isDeceased"><c>true</c> if contact is dead, otherwise <c>false</c></param>
        /// <param name="gender">Female, Male of Transgender</param>
        /// <param name="genderId">1, 2 or 3</param>
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate =
            "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=create&contact_type={contactType}&contact_id={id}&first_name={firstName}&last_name={lastName}&external_identifier={externalID}&birth_date={birthDate}&deceased_date={deceasedDate}&is_deceased={isDeceased}&gender={gender}&gender_id={genderId}")]
        void ContactSave(string apiKey, string key, int id, string firstName, string lastName, int externalId, ContactType contactType, DateTime? birthDate, DateTime? deceasedDate, bool isDeceased, Gender gender, int genderId);
    }
}
