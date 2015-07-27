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
    /// Custom fields for a CiviCRM membership.
    /// </summary>
    public partial class Membership
    {
        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name="custom_62"), JsonProperty]
        public bool VerzekeringLoonverlies { get; set; }

        [JsonConverter(typeof(NullableEnumConverter))]
        [DataMember(Name="custom_63"), JsonProperty]
        public FactuurStatus? FactuurStatus { get; set; }

        [DataMember(Name="custom_64_id")]
        public int? AangemaaktDoorPloegId { get; set; }
    }
}
