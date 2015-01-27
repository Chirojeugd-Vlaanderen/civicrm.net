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

using System.Collections.Generic;
using System.Runtime.Serialization;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Wcf.CustomFieldExample
{
    /// <summary>
    /// A relationship with a multi-value custom field.
    /// </summary>
    [CiviRequest]
    [DataContract]
    public class CustomRelatioship : Relationship
    {
        /// <summary>
        /// Functies van het lid, met gelijkaardige representatie als
        /// afdelingen. Bijvoorbeeld {{"GG1", "GG1"},{"FI", "FI"}}.
        /// </summary>
        [DataMember(Name = "custom_24"), JsonProperty]
        public Dictionary<string, string> Functies { get; set; }
    }
}
