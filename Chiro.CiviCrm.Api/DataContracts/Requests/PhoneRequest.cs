﻿/*
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

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A CiviCRM phone number.
    /// </summary>
    [DataContract]
    [CiviRequest]
    public class PhoneRequest : BaseRequest
    {
        [JsonProperty("contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }
        [JsonProperty("location_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? LocationTypeId { get; set; }
        [JsonProperty("is_primary", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? IsPrimary { get; set; }
        [JsonProperty("is_billing", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? IsBilling { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("phone_numeric", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumeric { get; set; }
        [JsonProperty("phone_type_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(NullableEnumConverter))]
        public PhoneType? PhoneType { get; set; }

        public override CiviEntity EntityType
        {
            get { return CiviEntity.Phone; }
        }
    }
}
