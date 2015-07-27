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

using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// Custom fields for a CiviCRM membership request.
    /// </summary>
    public partial class MembershipRequest
    {
        [JsonProperty("custom_62", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? VerzekeringLoonverlies { get; set; }

        [JsonProperty("custom_63", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(NullableEnumConverter))]
        public FactuurStatus? FactuurStatus { get; set; }

        [JsonProperty("custom_64", NullValueHandling = NullValueHandling.Ignore)]
        public int? AangemaaktDoorPloegId { get; set; }
    }
}
