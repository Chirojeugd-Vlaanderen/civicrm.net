/*
   Copyright 2015, 2016 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    // If you want to use custom fields of contacts, please add them here.
    // There is an example in the comments.
    // You could inherit from Contact as well, this would make retrieving the
    // custom fields a lot harder. And retrieving them in a chained call, would
    // be almost impossible.
    public partial class RelationshipRequest
    {
        /// <summary>
        /// Exact 1 afdeling. Als het om leiding of kader gaat,
        /// is de afdeling 'Leiding'.
        /// </summary>
        /// <remarks>
        /// Deze property is toch nullable, omdat we in CiviCRM niet kunnen
        /// afdwingen dat er precies 1 afdeling is.
        /// </remarks>
        [JsonProperty("custom_13", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(NullableEnumCharConverter))]
        public Afdeling? Afdeling { get; set; }

        /// <summary>
        /// Als de afdeling Leiding is, dan bepaalt deze property over welke
        /// afelingen de persoon leiding is.
        /// </summary>
        [JsonIgnore]
        public Afdeling[] LeidingVan { get; set; }

        [JsonProperty("custom_14", NullValueHandling = NullValueHandling.Ignore)]
        public char[] LeidingVanAfdelingChar
        {
            // Het zou leuker zijn moest ik die EnumCharConverter kunnen gebruiken
            // op de elementen van een array. Maar dat kreeg ik niet aan de praat.
            get { return LeidingVan == null ? null : LeidingVan.Select(lv => (char) (int) (lv)).ToArray(); }
        }

        /// <summary>
        /// Functies van het lid.
        /// </summary>
        [JsonProperty("custom_15", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Functies { get; set; }

        /// <summary>
        /// GAP-werkjaar van het lid.
        /// </summary>
        [JsonProperty("custom_78", NullValueHandling = NullValueHandling.Ignore)]
        public int? Werkjaar { get; set; }
    }
}
