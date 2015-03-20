/*
   Copyright 2014, 2015 Chirojeugd-Vlaanderen vzw

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

using System.Collections.Generic;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A CiviCRM address request.
    /// </summary>
    public partial class AddressRequest: BaseRequest
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public string IdValueExpression { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string IdString
        {
            get { return Id.HasValue ? Id.ToString() : IdValueExpression; }
        }
        [JsonProperty("contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }
        [JsonProperty("location_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? LocationTypeId { get; set; }
        [JsonProperty("is_primary", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPrimary { get; set; }
        [JsonProperty("is_billing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBilling { get; set; }
        [JsonProperty("street_address", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("postal_code", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }
        [JsonProperty("postal_code_suffix", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCodeSuffix { get; set; }
        [JsonProperty("state_province_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? StateProvinceId { get; set; }
        [JsonProperty("country_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CountryId { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        #region Chaining
        [JsonProperty("api.LocBlock.get", NullValueHandling = NullValueHandling.Ignore)]
        public LocBlockRequest LocBlockGetRequest { get; set; }
        [JsonConverter(typeof(Crm15815Converter))]
        [JsonProperty("api.LocBlock.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<LocBlockRequest> LocBlockSaveRequest { get; set; }
        #endregion
    }
}
