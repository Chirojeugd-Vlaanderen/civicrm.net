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
    /// <remarks>
    /// I use CiviCrmResponse as the result type, because I had problems with deserializing because the root element
    /// of every response of the CiviCRM API is ResultSet. WCF threw an exception because it didn't want to deserialize
    /// the same XML element to different data contracts.
    /// I used the workaround I found here:
    /// http://social.msdn.microsoft.com/Forums/vstudio/en-US/bcd031d7-c8a4-4bb0-8c85-bc5d7b46108a/rest-services-identical-xmlroot-attributes-on-different-classes
    /// but I am not sure whether this solution is OK.
    /// </remarks>
    [ServiceContract]
    [XmlSerializerFormat]
    public interface ICiviCrmApi: IDisposable
    {
        /// <summary>
        /// Find contact with given <paramref name="id"/>
        /// </summary>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="id">External ID of contact to be found</param>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <returns>If found, a set with the (unique) contact with 
        /// given <paramref name="id"/>,
        /// otherwise <c>null</c>.</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=get&contact_id={id}"
            )]
        CiviCrmResponse<ContactSet> ContactGet(string apiKey, string key, int id);

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
        CiviCrmResponse<ContactSet> ContactFind(string apiKey, string key, int externalId);

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

        /// <summary>
        /// Find the adresses of a contact with given <paramref name="contactId"/>.
        /// </summary>
        /// <param name="apiKey">API-key of the API user</param>
        /// <param name="key">Key of the CiviCRM installation</param>
        /// <param name="contactId">ID of the contact whose addresses are requested</param>
        /// <returns>List of addresses</returns>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=get&contact_id={contactId}")]
        CiviCrmResponse<AddressSet> ContactAddressesFind(string apiKey, string key, int contactId);


        /// <summary>
        /// Saves a new address or updates an existing address.
        /// </summary>
        /// <param name="apiKey">API-key of the API user</param>
        /// <param name="key">Key of the CiviCRM installation</param>
        /// <param name="id">Id of the address to be updated, or <c>0</c> if it is a new address.</param>
        /// <param name="contactId">Id of the contact the address applies to</param>
        /// <param name="locationTypeId">Location type. Should not be <c>0</c></param>
        /// <param name="isPrimary">Determines whether this address will be the primary address</param>
        /// <param name="isBilling">Determines whether this address will be the billing address</param>
        /// <param name="streetAddress">Street, number, suffix</param>
        /// <param name="city">City</param>
        /// <param name="stateProvinceId">CiviCRM StateProvindeId</param>
        /// <param name="postalCode">Postal code</param>
        /// <param name="postalCodeSuffix">Postal code suffix</param>
        /// <param name="countryId">CiviCRM country ID</param>
        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate =
            "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Address&action=create&id={Id}&contact_id={contactId}&location_type_id={locationTypeId}&is_primary={isPrimary}&is_billing={isBilling}&street_address={streetAddress}&city={city}&state_province_id={stateProvinceId}&postal_code={postalCode}&postal_code_suffix={postalCodeSuffix}&country_id={CountryId}"
            )]
        void AddressSave(string apiKey, string key, int id, int contactId, int locationTypeId, bool isPrimary,
            bool isBilling, string streetAddress, string city, int stateProvinceId, int postalCode,
            string postalCodeSuffix, int countryId);
    }
}
