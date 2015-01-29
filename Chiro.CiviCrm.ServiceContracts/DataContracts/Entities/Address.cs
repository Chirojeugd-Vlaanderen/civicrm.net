/*
   Copyright 2014 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM address.
    /// </summary>
    [DataContract]
    [CiviRequest]
    public class Address: BaseRequest
    {
        [DataMember(Name = "id"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [DataMember(Name = "contact_id"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }
        [DataMember(Name="location_type_id"), JsonProperty]
        public int LocationTypeId { get; set; }
        [DataMember(Name="is_primary"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsPrimary { get; set; }
        [DataMember(Name="is_billing"), JsonProperty]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsBilling { get; set; }
        [DataMember(Name="street_address"), JsonProperty]
        public string StreetAddress { get; set; }
        [DataMember(Name="city"), JsonProperty]
        public string City { get; set; }
        [DataMember(Name="postal_code"), JsonProperty]
        public string PostalCode { get; set; }
        [DataMember(Name="postal_code_suffix"), JsonProperty]
        public string PostalCodeSuffix { get; set; }
        [DataMember(Name="state_province_id"), JsonProperty]
        public int? StateProvinceId { get; set; }
        [DataMember(Name="country_id"), JsonProperty]
        public int? CountryId { get; set; }
        /// <summary>
        /// Name of country, or ISO-code
        /// </summary>
        /// <remarks>
        /// You can use this to create/update the country of an address.
        /// The CiviCRM address API doesn't seem to fetch the country.
        /// </remarks>
        [DataMember(Name="country"), JsonProperty]
        public string Country { get; set; }
    }
}
