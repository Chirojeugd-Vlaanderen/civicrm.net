﻿/*
   Copyright 2015, 2016 Chirojeugd-Vlaanderen vzw

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
using System.Linq;
using Chiro.CiviCrm.Api.Converters;
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A CiviCRM membership request.
    /// </summary>
    [CiviRequest]
    public partial class MembershipRequest : BaseRequest
    {
        [JsonProperty("contact_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContactId { get; set; }

        [JsonProperty("membership_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? MembershipTypeId { get; set; }

        [JsonProperty("join_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? JoinDate { get; set; }

        [JsonIgnore]
        public DateTime? StartDate { get; set; }

        [JsonIgnore]
        public Filter<DateTime?> StartDateFilter { get; set; }

        [JsonConverter(typeof(FilterConverter))]
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public Filter<DateTime?> StartDateOrStartDateFilter
        {
            get { return StartDateFilter ?? (StartDate == null ? null : new Filter<DateTime?>(StartDate)); }
        }

        [JsonIgnore]
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        public Filter<DateTime?> EndDateFilter { get; set; }

        [JsonConverter(typeof(FilterConverter))]
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public Filter<DateTime?> EndDateOrEndDateFilter
        {
            get { return EndDateFilter ?? (EndDate == null ? null : new Filter<DateTime?>(EndDate)); }
        }

        [JsonIgnore]
        public MembershipStatus? Status { get; set; }

        [JsonIgnore]
        public Filter<MembershipStatus> StatusFilter { get; set; }

        [JsonProperty("status_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof (FilterConverter))]
        public Filter<int> StatusOrFilter
        {
            get
            {
                if (StatusFilter != null)
                {
                    // I think I can't combine an enumconverter and a filterconverter,
                    // but I'm not sure. Working around the issue:
                    return new Filter<int>(StatusFilter.Operator,
                        StatusFilter.Values.Select(v => (int)v).ToArray());
                }
                return Status.HasValue ? new Filter<int>((int)Status.Value) : null;
            }
        }

        /// <summary>
        /// Only if IsOverride is true, the resulting membership will have the requested
        /// status. I will set this by default if Status != null.
        /// </summary>
        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("is_override", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOverride
        {
            get { return Status.HasValue ? true : (bool?)null; }
        }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("is_test", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTest { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("is_pay_later", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPayLater { get; set; }

        public override CiviEntity EntityType
        {
            get { return CiviEntity.Membership; }
        }

        #region Chaining
        [JsonProperty("api.MembershipPayment.get", NullValueHandling = NullValueHandling.Ignore)]
        public MembershipPaymentRequest MembershipPaymentGetRequest { get; set; }

        [JsonProperty("api.Contact.get", NullValueHandling = NullValueHandling.Ignore)]
        public ContactRequest ContactGetRequest { get; set; }
        #endregion
    }
}
