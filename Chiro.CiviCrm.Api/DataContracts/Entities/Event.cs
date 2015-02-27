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
using System.Runtime.Serialization;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM event.
    /// </summary>
    [DataContract]
    public partial class Event
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "event_type_id")]
        public int EventTypeId { get; set; }

        [DataMember(Name = "participant_listing_id")]
        public int? ParticipantListingId { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_public")]
        public bool IsPublic { get; set; }

        [JsonConverter(typeof(Crm15863Converter))]
        [DataMember(Name = "start_date")]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(Crm15863Converter))]
        [DataMember(Name = "end_date")]
        public DateTime EndDate { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_online_registration")]
        public bool IsOnlineRegistration { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_monetary")]
        public bool IsMonetary { get; set; }

        [DataMember(Name = "financial_type_id")]
        public int? FinancialTypeId { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_map")]
        public bool IsMap { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_active")]
        public bool IsActive { get; set; }

        [DataMember(Name = "fee_label")]
        public string FeeLabel { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_show_location")]
        public bool IsShowLocation { get; set; }

        [DataMember(Name = "default_role_id")]
        public int? DefaultRoleId { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_email_confirm")]
        public bool IsEmailConfirm { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_pay_later")]
        public bool IsPayLater { get; set; }

        [DataMember(Name = "pay_later_text")]
        public string PayLaterText { get; set; }

        [DataMember(Name = "pay_later_receipt")]
        public string PayLaterReceipt { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_partial_payment")]
        public bool IsPartialPayment { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_multiple_registrations")]
        public bool IsMultipleRegistrations { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "allow_same_participant_emails")]
        public bool AllowSameParticipantEmails { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "has_waitlist")]
        public bool HasWaitlist { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_template")]
        public bool IsTemplate { get; set; }

        [DataMember(Name = "created_id")]
        public int? CreatedId { get; set; }

        [JsonConverter(typeof(Crm15863Converter))]
        [DataMember(Name = "created_date")]
        public DateTime CreatedDate { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_share")]
        public bool IsShare { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [DataMember(Name = "is_confirm_enabled")]
        public bool IsConfirmEnabled { get; set; }

        [DataMember(Name = "contribution_type_id")]
        public int ContributionTypeId { get; set; }
    }
}
