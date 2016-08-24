﻿/*
   Copyright 2016 Chirojeugd-Vlaanderen vzw

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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// Request for ChiroWerkjaar.
    /// </summary>
    [CiviRequest]
    public class ChiroWerkjaarRequest: BaseRequest
    {
        [JsonProperty("contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }
        [JsonProperty("werkjaar", NullValueHandling = NullValueHandling.Ignore)]
        public int? Werkjaar { get; set; }
        [JsonProperty("stamnr", NullValueHandling = NullValueHandling.Ignore)]
        public string StamNummer { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
    }
}
