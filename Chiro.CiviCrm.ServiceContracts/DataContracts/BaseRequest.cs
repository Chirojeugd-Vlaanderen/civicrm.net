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

using System;
using System.Collections.Generic;
using System.Linq;
using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// A class that basically converts a request to the json-part of the url.
    /// If you use CiviRequest, all entities will be fetched, with a maximum
    /// of 25. If you want to search for specific entities, you should inherit
    /// CiviRequest. (See e.g. CiviExternalIdentifierRequest.)
    /// </summary>
    [JsonConvertible]
    public class BaseRequest: ICustomJsonConversion
    {
        /// <summary>
        /// Fields to return after the call.
        /// </summary>
        [JsonProperty("return", NullValueHandling = NullValueHandling.Ignore)]
        public string ReturnFields { get; set; }

        /// <summary>
        /// Options to pass to the CiviCRM-API
        /// </summary>
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public ApiOptions ApiOptions { get; set; }

        /// <summary>
        /// This should be one, for correctly deserializing the API results.
        /// </summary>
        [JsonProperty("sequential")]
        public int Sequential { get { return 1; } }

        /// <summary>
        /// Indicates which chained entities to get.
        /// </summary>
        [JsonIgnore]
        public CiviEntity[] ChainedGet { get; set; }

        /// <summary>
        /// Entities to be created in a chained call.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<IEntity> ChainedCreate { get; set; }

        /// <summary>
        /// Dummy property we will serialize if we need to chain.
        /// </summary>
        [JsonProperty("chains.placeholder", NullValueHandling = NullValueHandling.Ignore)]
        public int? ChainsPlaceholder { get; private set; }

        public BaseRequest()
        {
            ChainsPlaceholder = null;
        }

        /// <summary>
        /// Converts the request to Json for the API.
        /// </summary>
        /// <returns>A Json-representation of this request.</returns>
        public string ToJson()
        {
            // There might be some functionality in Json.Net that can do this
            // in a more elegant way.

            string chains;

            if ((ChainedGet == null || !ChainedGet.Any()) && (ChainedCreate == null || !ChainedCreate.Any()))
            {
                chains = String.Empty;
                ChainsPlaceholder = null;
            }
            else
            {
                IEnumerable<String> getChains = new string[0];
                IEnumerable<String> createChains = new string[0];

                if (ChainedGet != null)
                {
                    getChains = from entity in ChainedGet
                            select String.Format("\"api.{0}.get\":{{}}", entity);
                }

                if (ChainedCreate != null)
                {
                    createChains = from entity in ChainedCreate
                        group entity by entity.GetType()
                        into g
                        select
                            String.Format("\"api.{0}.create\":{1}", g.Key.Name,
                                JsonConvert.SerializeObject(g));
                }
                chains = String.Join(",", getChains.Union(createChains));
                ChainsPlaceholder = 1;
            }
            string json = JsonConvert.SerializeObject(this);

            // if no chaining is needed, chains.placeholder will not be
            // serialized. In that case the replace command below won't do a thing.
            json = json.Replace("\"chains.placeholder\":1", chains);

            return json;
        }
    }
}
