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

using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A class that basically converts a request to the json-part of the url.
    /// If you use CiviRequest, all entities will be fetched, with a maximum
    /// of 25. If you want to search for specific entities, you should inherit
    /// CiviRequest. (See e.g. CiviExternalIdentifierRequest.)
    /// </summary>
    [JsonConvertible]
    public class CiviRequest: ICustomJsonConversion
    {
        [JsonIgnore]
        public string[] chained_entities { get; set; }

        [JsonProperty("return", NullValueHandling = NullValueHandling.Ignore)]
        public string return_values { get; set; }

        public int sequential { get; set; }
        /// <summary>
        /// Dummy property we will serialize if we need to chain.
        /// </summary>
        [JsonProperty("chains.placeholder", NullValueHandling = NullValueHandling.Ignore)]
        public int? chains_placeholder { get; set; }

        public CiviRequest()
        {
            chains_placeholder = null;
            sequential = 1;
        }

        public string ToJson()
        {
            // There might be some functionality in Json.Net that can do this
            // in a more elegant way.

            string chains;

            if (chained_entities == null)
            {
                chains = String.Empty;
                chains_placeholder = null;
            }
            else
            {
                var parts = from entity in chained_entities
                            select String.Format("\"api.{0}.get\":{{}}", entity);
                chains = String.Join(",", parts);
                chains_placeholder = 1;
            }
            string json = JsonConvert.SerializeObject(this);

            // if no chaining is needed, chains.placeholder will not be
            // serialized. In that case the replace command below won't do a thing.
            json = json.Replace("\"chains.placeholder\":1", chains);

            return json;
        }
    }
}
