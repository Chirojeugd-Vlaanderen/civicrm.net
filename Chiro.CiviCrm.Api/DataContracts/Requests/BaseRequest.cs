/*
   Copyright 2014, 2015, 2016 Chirojeugd-Vlaanderen vzw

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
    /// A class that basically converts a request to the json-part of the url.
    /// If you use CiviRequest, all entities will be fetched, with a maximum
    /// of 25. If you want to search for specific entities, you should inherit
    /// CiviRequest. (See e.g. CiviExternalIdentifierRequest.)
    /// </summary>
    [CiviRequest]
    public class BaseRequest
    {
        /// <summary>
        /// Id of the entity to fetch (if given)
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }
        /// <summary>
        /// Value expression for Id. Only for chaining.
        /// </summary>
        [JsonIgnore]
        public string IdValueExpression { get; set; }
        /// <summary>
        /// Id if given, otherwise IdValueExpression. For the API.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string IdString
        {
            get { return Id.HasValue ? Id.ToString() : IdValueExpression; }
        }

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
        /// The entity type this request is referring to.
        /// </summary>
        [JsonIgnore]
        public virtual CiviEntity EntityType { get; }
    }
}
