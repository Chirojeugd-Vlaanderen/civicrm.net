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


using System;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A CiviCRM membership request.
    /// </summary>
    [CiviRequest]
    public class MembershipRequest: BaseRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }

        [JsonProperty("membership_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? MembershipTypeId { get; set; }

        [JsonProperty("join_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? JoinDate { get; set; }

        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        [JsonProperty("status_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(NullableEnumConverter))]
        public MembershipStatus? Status { get; set; }

        /// <summary>
        /// Only if IsOverride is true, the resulting membership will have the requested
        /// status. I will set this by default if Status != null.
        /// </summary>
        [JsonConverter(typeof (BoolConverter))]
        [JsonProperty("is_override", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOverride
        {
            get { return Status.HasValue ? true : (bool?) null; }
        }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("is_test", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTest { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("is_pay_later", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPayLater { get; set; }
    }
}
