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
    public class CiviContact
    {
        [DataMember, JsonProperty]
        public int? id { get; set; }

        [DataMember, JsonProperty]
        public ContactType contact_type { get; set; }

        [DataMember, JsonProperty]
        public string contact_sub_type { get; set; }

        [DataMember, JsonProperty]
        public string sort_name { get; set; }

        [DataMember, JsonProperty]
        public string display_name { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool do_not_email { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember, JsonProperty]
        public bool do_not_phone { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember, JsonProperty]
        public bool do_not_sms { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember, JsonProperty]
        public bool do_not_trade { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember, JsonProperty]
        public bool is_opt_out { get; set; }

        [DataMember, JsonProperty]
        public string legal_identifier { get; set; }

        [DataMember, JsonProperty]
        public string external_identifier { get; set; }

        [DataMember, JsonProperty]
        public string nick_name { get; set; }

        [DataMember, JsonProperty]
        public string legal_name { get; set; }

        [DataMember, JsonProperty]
        public string image_URL { get; set; }

        // preferred communication method is too much hassle.
        //[DataMember, JsonProperty]
        //public IEnumerable<int> preferred_communication_method { get; set; }

        [DataMember, JsonProperty]
        public string preferred_language { get; set; }

        [DataMember, JsonProperty]
        public MailFormat preferred_mail_format { get; set; }

        [DataMember, JsonProperty]
        public string first_name { get; set; }

        [DataMember, JsonProperty]
        public string middle_name { get; set; }

        [DataMember, JsonProperty]
        public string last_name { get; set; }

        [DataMember, JsonProperty]
        public string formal_title { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(EnumIntConverter))]
        public CommunicationStyle communication_style_id { get; set; }

        [DataMember, JsonProperty]
        public string job_title { get; set; }

        [DataMember, JsonProperty]
        //[JsonConverter(typeof(EnumIntConverter))]
        public Gender gender_id { get; set; }

        [DataMember, JsonProperty]
        public DateTime? birth_date { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool is_deceased { get; set; }

        [DataMember, JsonProperty]
        public DateTime? deceased_date { get; set; }

        [DataMember, JsonProperty]
        public string household_name { get; set; }

        [DataMember, JsonProperty]
        public string organization_name { get; set; }

        [DataMember, JsonProperty]
        public string sic_code  { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool contact_is_deleted  { get; set; }

        [DataMember, JsonProperty]
        public string current_employer { get; set; }

        [DataMember, JsonProperty]
        public int? address_id { get; set; }

        [DataMember, JsonProperty]
        public string street_address { get; set; }

        [DataMember, JsonProperty]
        public string supplemental_address_1 { get; set; }

        [DataMember, JsonProperty]
        public string supplemental_address_2 { get; set; }

        [DataMember, JsonProperty]
        public string city { get; set; }

        [DataMember, JsonProperty]
        public string postal_code_suffix { get; set; }

        [DataMember, JsonProperty]
        public string postal_code { get; set; }

        [DataMember, JsonProperty]
        public string geo_code_1 { get; set; }

        [DataMember, JsonProperty]
        public string geo_code_2 { get; set; }

        [DataMember, JsonProperty]
        public int? phone_id { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(EnumIntConverter))]
        public PhoneType? phone_type_id { get; set; }

        [DataMember, JsonProperty]
        public string phone { get; set; }

        [DataMember, JsonProperty]
        public int? email_id { get; set; }

        [DataMember, JsonProperty]
        public string email { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool on_hold { get; set; }

        [DataMember, JsonProperty]
        public int? im_id { get; set; }

        [DataMember, JsonProperty]
        [JsonConverter(typeof(EnumIntConverter))]
        public Provider? provider_id { get; set; }

        [DataMember, JsonProperty]
        public string im { get; set; }

        [DataMember, JsonProperty]
        public string world_region { get; set; }

        [DataMember, JsonProperty]
        public string individual_prefix { get; set; }

        [DataMember, JsonProperty]
        public string individual_suffix { get; set; }

        [DataMember, JsonProperty]
        public string state_province_name { get; set; }

        [DataMember, JsonProperty]
        public string country { get; set; }

        [DataMember(Name="api.Address.get"), JsonProperty]
        public CiviResultValues<CiviAddress> chained_addresses { get; set; }

        // Options are relevant for updates. Semantically they do not
        // belong in this data contract, but the CiviCRM API expects
        // them here.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CiviApiOptions options { get; set; }
    }
}
