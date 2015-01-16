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

using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Request for searching a contact based on its external identifier.
    /// </summary>
    public class ExternalIdentifierRequest: BaseRequest
    {
        [JsonProperty("external_identifier")]
        public string ExternalIdentifier { get; set; }

        /// <summary>
        /// Most of the time this will be <c>null</c>. But you can use this
        /// property if you want to change only the external ID of a contact.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        public ExternalIdentifierRequest() : base() { }

        public ExternalIdentifierRequest(string externalIdentifier)
            : this()
        {
            ExternalIdentifier = externalIdentifier;
        }
    }
}
