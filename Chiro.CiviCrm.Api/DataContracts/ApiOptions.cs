﻿/*
   Copyright 2014-2015 Chirojeugd-Vlaanderen vzw

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

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Options to pass to the CiviCRM API.
    /// </summary>
    /// <remarks>
    /// match and sort work with field names from civicrm. (lower case and underscores)
    /// </remarks>
    public class ApiOptions
    {
        [JsonProperty("match", NullValueHandling = NullValueHandling.Ignore)]
        public string Match { get; set; }

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        public string Sort { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        [JsonProperty("offset", NullValueHandling = NullValueHandling.Ignore)]
        public int? Offset { get; set; }
    }
}
