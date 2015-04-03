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

using System.Runtime.Serialization;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM phone number.
    /// </summary>
    [DataContract]
    public class Phone
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "contact_id")]
        public int? ContactId { get; set; }
        [DataMember(Name = "location_type_id")]
        public int LocationTypeId { get; set; }
        [DataMember(Name = "is_primary")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsPrimary { get; set; }
        [DataMember(Name = "is_billing")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsBilling { get; set; }
        [DataMember(Name = "phone")]
        public string PhoneNumber { get; set; }
        [DataMember(Name = "phone_numeric")]
        public string PhoneNumeric { get; set; }
        [DataMember(Name = "phone_type_id")]
        [JsonConverter(typeof(NullableEnumConverter))]
        public PhoneType? PhoneType { get; set; }
    }
}
