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

using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// Chirocivi custom fields for event requests.
    /// </summary>
    public partial class EventRequest
    {
        [JsonProperty("custom_50", NullValueHandling = NullValueHandling.Ignore)]
        public string KipId { get; set; }

        [JsonProperty("custom_51", NullValueHandling = NullValueHandling.Ignore)]
        public string BronBoekhouding { get; set; }

        [JsonProperty("custom_52", NullValueHandling = NullValueHandling.Ignore)]
        public string AnalytischeCode { get; set; }

        [JsonProperty("custom_53", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePersoon1Id { get; set; }

        [JsonProperty("custom_54", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePersoon2Id { get; set; }

        [JsonProperty("custom_55", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePersoon3Id { get; set; }

        [JsonProperty("custom_56", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePloeg1Id { get; set; }

        [JsonProperty("custom_57", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePloeg2Id { get; set; }

        [JsonProperty("custom_58", NullValueHandling = NullValueHandling.Ignore)]
        public int? OrganiserendePloeg3Id { get; set; }

        [JsonProperty("custom_59", NullValueHandling = NullValueHandling.Ignore)]
        public int? AantalVormingsUren { get; set; }

        [JsonProperty("custom_60", NullValueHandling = NullValueHandling.Ignore)]
        public string OfficieelCursusNummer { get; set; }

        [JsonProperty("custom_61", NullValueHandling = NullValueHandling.Ignore)]
        public int? GapUitstapId { get; set; }
    }
}
