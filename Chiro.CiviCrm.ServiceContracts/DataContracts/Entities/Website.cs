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
using Chiro.CiviCrm.BehaviorExtension;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM Website.
    /// </summary>
    [DataContract]
    [JsonConvertible]
    public class Website : BaseRequest, IEntity
    {
        [DataMember(Name = "id"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }
        [DataMember(Name = "contact_id"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }
        [DataMember(Name = "url"), JsonProperty]
        public string Url { get; set; }
        [DataMember(Name = "website_type_id"), JsonProperty]
        [JsonConverter(typeof(NullableEnumConverter))]
        public WebsiteType? WebsiteType { get; set; }
    }
}
