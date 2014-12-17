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
        [DataMember]
        public int? id { get; set; }

        [DataMember]
        public ContactType contact_type { get; set; }

        [DataMember]
        public string contact_sub_type { get; set; }

        [DataMember]
        public string sort_name { get; set; }

        [DataMember]
        public string display_name { get; set; }

        [DataMember]
        public bool do_not_email { get; set; }

        [DataMember]
        public bool do_not_phone { get; set; }

        [DataMember]
        public bool do_not_sms { get; set; }

        [DataMember]
        public bool do_not_trade { get; set; }

        [DataMember]
        public bool is_opt_out { get; set; }

        [DataMember]
        public string legal_identifier { get; set; }

        [DataMember]
        public string external_identifier { get; set; }

        [DataMember]
        public string nick_name { get; set; }

        [DataMember]
        public string legal_name { get; set; }

        [DataMember]
        public string image_URL { get; set; }

        // preferred communication method is too much hassle.
        //[DataMember]
        //public IEnumerable<int> preferred_communication_method { get; set; }

        [DataMember]
        public string preferred_language { get; set; }

        [DataMember]
        public MailFormat preferred_mail_format { get; set; }

        [DataMember]
        public string first_name { get; set; }

        [DataMember]
        public string middle_name { get; set; }

        [DataMember]
        public string last_name { get; set; }

        [DataMember]
        public string formal_title { get; set; }

        [DataMember]
        public CommunicationStyle communication_style_id { get; set; }

        [DataMember]
        public string job_title { get; set; }

        [DataMember]
        public Gender gender_id { get; set; }

        [DataMember]
        public DateTime? birth_date { get; set; }

        [DataMember]
        public bool is_deceased { get; set; }

        [DataMember]
        public DateTime? deceased_date { get; set; }

        [DataMember]
        public string household_name { get; set; }

        [DataMember]
        public string organization_name { get; set; }

        [DataMember]
        public string sic_code  { get; set; }

        [DataMember]
        public bool contact_is_deleted  { get; set; }

        [DataMember]
        public string current_employer { get; set; }

        [DataMember]
        public int? address_id { get; set; }

        [DataMember]
        public string street_address { get; set; }

        [DataMember]
        public string supplemental_address_1 { get; set; }

        [DataMember]
        public string supplemental_address_2 { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string postal_code_suffix { get; set; }

        [DataMember]
        public string postal_code { get; set; }

        [DataMember]
        public string geo_code_1 { get; set; }

        [DataMember]
        public string geo_code_2 { get; set; }

        [DataMember]
        public int? phone_id { get; set; }

        [DataMember]
        public PhoneType? phone_type_id { get; set; }

        [DataMember]
        public string phone { get; set; }

        [DataMember]
        public int? email_id { get; set; }

        [DataMember]
        public string email { get; set; }

        [DataMember]
        public bool? on_hold { get; set; }

        [DataMember]
        public int? im_id { get; set; }

        [DataMember]
        public Provider? provider_id { get; set; }

        [DataMember]
        public string im { get; set; }

        [DataMember]
        public string world_region { get; set; }

        [DataMember]
        public string individual_prefix { get; set; }

        [DataMember]
        public string individual_suffix { get; set; }

        [DataMember]
        public string state_province_name { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember(Name="api.Address.get")]
        public CiviResultValues<CiviAddress> chained_addresses { get; set; }

        // Options are relevant for updates. Semantically they do not
        // belong in this data contract, but the CiviCRM API expects
        // them here.
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public CiviApiOptions options { get; set; }
    }
}
