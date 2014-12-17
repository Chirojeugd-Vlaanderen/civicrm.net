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

using Chiro.CiviCrm.Api.Converters;
using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace Chiro.CiviCrm.Api.DataContracts
{
    // Except for the contact id, there are no nullable ints or bools
    // in the data contract. When an int/bool is missing, CiviCRM
    // returns an empty string, which cannot be mapped automatically.
    [DataContract]
    [JsonConvertible]
    public class Contact
    {
        [DataMember(Name="id"), JsonProperty]
        public int? Id { get; set; }

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

        // preferred communication method is too much hassle.
        //[DataMember, JsonProperty]
        //public IEnumerable<int> preferred_communication_method { get; set; }

        [DataMember(Name="preferred_language"), JsonProperty]
        public string PreferredLanguage { get; set; }

        [DataMember(Name="preferred_mail_format"), JsonProperty]
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
        [JsonConverter(typeof(EnumIntConverter))]
        public CommunicationStyle CommunicationStyle { get; set; }

        [DataMember(Name="job_title"), JsonProperty]
        public string JobTitle { get; set; }

        [DataMember(Name="gender_id"), JsonProperty]
        //[JsonConverter(typeof(EnumIntConverter))]
        public Gender Gender { get; set; }

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

        [DataMember(Name="address_id"), JsonProperty]
        public int? AddressId { get; set; }

        [DataMember(Name="street_address"), JsonProperty]
        public string StreetAddress { get; set; }

        [DataMember(Name="supplemental_address_1"), JsonProperty]
        public string SupplementalAddress1 { get; set; }

        [DataMember(Name="supplemental_address_2"), JsonProperty]
        public string SupplementalAddress2 { get; set; }

        [DataMember(Name="city"), JsonProperty]
        public string City { get; set; }

        [DataMember(Name="postal_code_suffix"), JsonProperty]
        public string PostalCodeSuffix { get; set; }

        [DataMember(Name="postal_code"), JsonProperty]
        public string PostalCode { get; set; }

        [DataMember(Name="geo_code_1"), JsonProperty]
        public string GeoCode1 { get; set; }

        [DataMember(Name="geo_code_2"), JsonProperty]
        public string GeoCode2 { get; set; }

        [DataMember(Name="phone_id"), JsonProperty]
        public int? PhoneId { get; set; }

        [DataMember(Name="phone_type_id"), JsonProperty]
        [JsonConverter(typeof(EnumIntConverter))]
        public PhoneType? PhoneType { get; set; }

        [DataMember(Name="phone"), JsonProperty]
        public string Phone { get; set; }

        [DataMember(Name="email_id"), JsonProperty]
        public int? EmailId { get; set; }

        [DataMember(Name="email"), JsonProperty]
        public string Email { get; set; }

        [DataMember(Name="on_hold"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool OnHold { get; set; }

        [DataMember(Name="im_id"), JsonProperty]
        public int? ImId { get; set; }

        [DataMember(Name="provider_id"), JsonProperty]
        [JsonConverter(typeof(EnumIntConverter))]
        public Provider? Provider { get; set; }

        [DataMember(Name="im"), JsonProperty]
        public string Im { get; set; }

        [DataMember(Name="world_region"), JsonProperty]
        public string WorldRegion { get; set; }

        [DataMember(Name="individual_prefix"), JsonProperty]
        public string IndividualPrefix { get; set; }

        [DataMember(Name="individual_suffix"), JsonProperty]
        public string IndividualSuffix { get; set; }

        [DataMember(Name="state_province_name"), JsonProperty]
        public string StateProvinceName { get; set; }

        [DataMember(Name="country"), JsonProperty]
        public string Country { get; set; }

        [DataMember(Name="api.Address.get"), JsonProperty]
        public CiviResultValues<Address> ChainedAddresses { get; set; }

        // Options are relevant for updates. Semantically they do not
        // belong in this data contract, but the CiviCRM API expects
        // them here.
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public ApiOptions ApiOptions { get; set; }
    }
}
