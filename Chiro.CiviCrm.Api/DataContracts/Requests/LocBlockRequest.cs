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
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// Request for a CiviCRM LocBlock.
    /// </summary>
    /// <remarks>
    /// I did not invent this :-)
    /// </remarks>
    public partial class LocBlockRequest: BaseRequest
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public string IdValueExpression { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string IdString
        {
            get { return Id.HasValue ? Id.ToString() : IdValueExpression; }
        }
        [JsonIgnore]
        public int? AddressId { get; set; }
        [JsonIgnore]
        public string AddressIdValueExpression { get; set; }
        [JsonProperty("address_id", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressIdString
        {
            get { return AddressId.HasValue ? AddressId.ToString() : AddressIdValueExpression; }
        }
        [JsonProperty("email_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EmailId { get; set; }
        [JsonProperty("phone_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? PhoneId { get; set; }
        [JsonProperty("im_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ImId { get; set; }
        [JsonProperty("address_2_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Address2Id { get; set; }
        [JsonProperty("email_2_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Email2Id { get; set; }
        [JsonProperty("phone_2_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Phone2Id { get; set; }
        [JsonProperty("im_2_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Im2Id { get; set; }
        /// <summary>
        /// You cannot use chaining to create a LocBlock together with an address.
        /// But there is a workaround.
        /// See https://issues.civicrm.org/jira/browse/CRM-14158
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public AddressRequest Address { get; set; }

        #region Chaining
        [JsonProperty("api.Address.get", NullValueHandling = NullValueHandling.Ignore)]
        public AddressRequest AddressGetRequest { get; set; }

        [JsonProperty("api.Event.get", NullValueHandling = NullValueHandling.Ignore)]
        public EventRequest EventGetRequest { get; set; }

        [JsonConverter(typeof(Crm15815Converter))]
        [JsonProperty("api.Event.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<EventRequest> EventSaveRequest { get; set; }
        #endregion

        public override CiviEntity EntityType
        {
            get { return CiviEntity.LocBlock; }
        }
    }
}
