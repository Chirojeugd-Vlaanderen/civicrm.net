﻿/*
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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// A CiviCRM event request.
    /// </summary>
    public partial class EventRequest: BaseRequest
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("event_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? EventTypeId { get; set; }

        [JsonProperty("participant_listing_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ParticipantListingId { get; set; }

        [JsonProperty("is_public", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPublic { get; set; }

        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        [JsonProperty("is_online_registration", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOnlineRegistration { get; set; }

        [JsonProperty("is_monetary", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMonetary { get; set; }

        [JsonProperty("financial_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? FinancialTypeId { get; set; }

        [JsonProperty("is_map", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMap { get; set; }

        [JsonProperty("is_active", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsActive { get; set; }

        [JsonProperty("fee_label", NullValueHandling = NullValueHandling.Ignore)]
        public string FeeLabel { get; set; }

        [JsonProperty("is_show_location", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsShowLocation { get; set; }

        [JsonProperty("default_role_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? DefaultRoleId { get; set; }

        [JsonProperty("is_email_confirm", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsEmailConfirm { get; set; }

        [JsonProperty("is_pay_later", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPayLater { get; set; }

        [JsonProperty("pay_later_text", NullValueHandling = NullValueHandling.Ignore)]
        public string PayLaterText { get; set; }

        [JsonProperty("pay_later_receipt", NullValueHandling = NullValueHandling.Ignore)]
        public string PayLaterReceipt { get; set; }

        [JsonProperty("is_partial_payment", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPartialPayment { get; set; }

        [JsonProperty("is_multiple_registrations", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMultipleRegistrations { get; set; }

        [JsonProperty("allow_same_participant_emails", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowSameParticipantEmails { get; set; }

        [JsonProperty("has_waitlist", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasWaitlist { get; set; }

        [JsonProperty("is_template", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTemplate { get; set; }

        [JsonProperty("created_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? CreatedId { get; set; }

        [JsonProperty("created_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedDate { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("is_share", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsShare { get; set; }

        [JsonProperty("is_confirm_enabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsConfirmEnabled { get; set; }

        [JsonProperty("contribution_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ContributionTypeId { get; set; }
    }
}
