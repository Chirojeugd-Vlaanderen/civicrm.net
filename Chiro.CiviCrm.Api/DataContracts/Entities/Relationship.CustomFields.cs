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

using System.Linq;
using System.Runtime.Serialization;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    // If you want to use custom fields of contacts, please add them here.
    // There is an example in the comments.
    // You could inherit from Contact as well, this would make retrieving the
    // custom fields a lot harder. And retrieving them in a chained call, would
    // be almost impossible.
    public partial class Relationship
    {
        /// <summary>
        /// Exact 1 afdeling. Als het om leiding of kader gaat,
        /// is de afdeling 'Leiding'.
        /// </summary>
        /// <remarks>
        /// Deze property is toch nullable, omdat we in CiviCRM niet kunnen
        /// afdwingen dat er precies 1 afdeling is.
        /// </remarks>
        [JsonConverter(typeof(EnumCharConverter))]
        [DataMember(Name = "custom_22"), JsonProperty]
        public Afdeling? Afdeling { get; set; }

        /// <summary>
        /// Als de afdeling Leiding is, dan bepaalt deze property over welke
        /// afelingen de persoon leiding is.
        /// </summary>
        [JsonIgnore]
        public Afdeling[] LeidingVan { get; set; }

        [DataMember(Name = "custom_23"), JsonProperty]
        public char[] LeidingVanAfdelingChar
        {
            set { LeidingVan = value.Select(v => (Afdeling)((int)v)).ToArray(); }
        }


        /// <summary>
        /// Functies van het lid.
        /// </summary>
        [DataMember(Name = "custom_24"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Functies { get; set; } 
    }
}
