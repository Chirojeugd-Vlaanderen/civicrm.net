/*
   Copyright 2015 Chirojeugd-Vlaanderen vzw

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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM membership.
    /// </summary>
    [DataContract]
    public class Membership
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "contact_id")]
        public int ContactId { get; set; }

        [DataMember(Name = "membership_type_id"), JsonProperty]
        public int MembershipTypeId { get; set; }

        [DataMember(Name = "join_date")]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? JoinDate { get; set; }

        [DataMember(Name = "start_date")]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "end_date")]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "status_id")]
        [JsonConverter(typeof(NullableEnumConverter))]
        public MembershipStatus Status { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_override")]
        public bool? IsOverride { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_test")]
        public bool? IsTest { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_pay_later")]
        public bool? IsPayLater { get; set; }
    }
}
