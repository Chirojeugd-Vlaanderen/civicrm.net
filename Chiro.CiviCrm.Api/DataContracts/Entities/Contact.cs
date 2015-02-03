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
using System.Runtime.Serialization;
using Chiro.CiviCrm.Api.Converters;
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM contact
    /// </summary>
    [DataContract]
    [CiviRequest]
    public partial class Contact
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [DataMember(Name="contact_type"), JsonProperty]
        public ContactType ContactType { get; set; }

        [DataMember(Name="contact_sub_type"), JsonProperty]
        public string ContactSubType { get; set; }

        [DataMember(Name="sort_name"), JsonProperty]
        public string SortName { get; set; }

        [DataMember(Name="display_name"), JsonProperty]
        public string DisplayName { get; set; }

        [DataMember(Name="do_not_email"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool DoNotEmail { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name="do_not_phone"), JsonProperty]
        public bool DoNotPhone { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name="do_not_sms"), JsonProperty]
        public bool DoNotSms { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name="do_not_trade"), JsonProperty]
        public bool DoNotTrade { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name="is_opt_out"), JsonProperty]
        public bool IsOptOut { get; set; }

        [DataMember(Name="legal_identifier"), JsonProperty]
        public string LegalIdentifier { get; set; }

        [DataMember(Name="external_identifier"), JsonProperty]
        public string ExternalIdentifier { get; set; }

        [DataMember(Name="nick_name"), JsonProperty]
        public string NickName { get; set; }

        [DataMember(Name="legal_name"), JsonProperty]
        public string LegalName { get; set; }

        [DataMember(Name="image_URL"), JsonProperty]
        public string ImageUrl { get; set; }

        [DataMember(Name="preferred_communication_method"), JsonProperty]
        public int[] PreferredCommunicationMethod { get; set; }

        [DataMember(Name="preferred_language"), JsonProperty]
        public string PreferredLanguage { get; set; }

        [DataMember(Name="preferred_mail_format"), JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public MailFormat PreferredMailFormat { get; set; }

        [DataMember(Name="first_name"), JsonProperty]
        public string FirstName { get; set; }

        [DataMember(Name="middle_name"), JsonProperty]
        public string MiddleName { get; set; }

        [DataMember(Name="last_name"), JsonProperty]
        public string LastName { get; set; }

        [DataMember(Name="formal_title"), JsonProperty]
        public string FormalTitle { get; set; }

        [DataMember(Name="communication_style_id"), JsonProperty]
        [JsonConverter(typeof(NullableEnumConverter))]
        public CommunicationStyle? CommunicationStyle { get; set; }

        [DataMember(Name="job_title"), JsonProperty]
        public string JobTitle { get; set; }

        [DataMember(Name="gender_id"), JsonProperty]
        [JsonConverter(typeof(NullableEnumConverter))]
        public Gender? Gender { get; set; }

        [DataMember(Name="birth_date"), JsonProperty]
        public DateTime? BirthDate { get; set; }
        
        [DataMember(Name="is_deceased"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsDeceased { get; set; }

        [DataMember(Name="deceased_date"), JsonProperty]
        public DateTime? DeceasedDate { get; set; }

        [DataMember(Name="household_name"), JsonProperty]
        public string HouseholdName { get; set; }

        [DataMember(Name="organization_name"), JsonProperty]
        public string OrganizationName { get; set; }

        [DataMember(Name="sic_code"), JsonProperty]
        public string SicCode  { get; set; }

        [DataMember(Name="contact_is_deleted"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool ContactIsDeleted { get; set; }

        [DataMember(Name="current_employer"), JsonProperty]
        public string CurrentEmployer { get; set; }

        [DataMember(Name = "address_id")]
        public int? AddressId { get; set; }

        [DataMember(Name = "street_address")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "supplemental_address_1")]
        public string SupplementalAddress1 { get; set; }

        [DataMember(Name = "supplemental_address_2")]
        public string SupplementalAddress2 { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "postal_code_suffix")]
        public string PostalCodeSuffix { get; set; }

        [DataMember(Name = "postal_code")]
        public string PostalCode { get; set; }

        [DataMember(Name = "geo_code_1")]
        public string GeoCode1 { get; set; }

        [DataMember(Name = "geo_code_2")]
        public string GeoCode2 { get; set; }

        [DataMember(Name = "phone_id")]
        public int? PhoneId { get; set; }

        [DataMember(Name = "phone_type_id")]
        [JsonConverter(typeof(NullableEnumConverter))]
        public PhoneType? PhoneType { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        [DataMember(Name = "email_id")]
        public int? EmailId { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "on_hold")]
        [JsonConverter(typeof(BoolConverter))]
        public bool OnHold { get; set; }

        [DataMember(Name = "im_id")]
        public int? ImId { get; set; }

        [DataMember(Name = "provider_id")]
        [JsonConverter(typeof(NullableEnumConverter))]
        public Provider? Provider { get; set; }

        [DataMember(Name = "im")]
        public string Im { get; set; }

        [DataMember(Name="world_region")]
        public string WorldRegion { get; set; }

        [DataMember(Name="individual_prefix")]
        public string IndividualPrefix { get; set; }

        [DataMember(Name="individual_suffix")]
        public string IndividualSuffix { get; set; }

        [DataMember(Name = "state_province_name")]
        public string StateProvinceName { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        #region Chaining
        [DataMember(Name = "api.Address.get")]
        public ApiResultValues<Address> AddressResult { get; set; }
        [DataMember(Name = "api.Phone.get")]
        public ApiResultValues<Phone> PhoneResult { get; set; }
        [DataMember(Name = "api.Email.get")]
        public ApiResultValues<Email> EmailResult { get; set; }
        [DataMember(Name = "api.Website.get")]
        public ApiResultValues<Website> WebsiteResult { get; set; }
        [DataMember(Name = "api.Im.get")]
        public ApiResultValues<Im> ImResult { get; set; }
        [DataMember(Name = "api.Relationship.get")]
        public ApiResultValues<Relationship> RelationshipResult { get; set; }
        #endregion
    }
}
