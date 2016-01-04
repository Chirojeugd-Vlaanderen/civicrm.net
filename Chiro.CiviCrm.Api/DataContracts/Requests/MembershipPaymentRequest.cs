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

using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    [CiviRequest]
    public class MembershipPaymentRequest : BaseRequest
    {
        [JsonProperty("membership_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? MembershipId { get; set; }

        [JsonProperty("contribution_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContributionId { get; set; }

        public override CiviEntity EntityType
        {
            get
            {
                return CiviEntity.MembershipPayment;
            }
        }
    }
}
