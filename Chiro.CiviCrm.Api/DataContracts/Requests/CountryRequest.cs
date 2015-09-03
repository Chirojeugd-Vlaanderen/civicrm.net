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

using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    public class CountryRequest: BaseRequest
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("iso_code", NullValueHandling = NullValueHandling.Ignore)]
        public string IsoCode { get; set; }
        [JsonProperty("region_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RegionId { get; set; }
        [JsonProperty("is_province_abbreviated", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsProvinceAbbreviated { get; set; }

        /// <summary>
        /// The entity type this request is referring to.
        /// </summary>
        public override CiviEntity EntityType
        {
            get { return CiviEntity.Country; }
        }
    }
}
