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
using System.Collections.Generic;
using System.Linq;
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    /// <summary>
    /// Information for a get or create operation on a contact.
    /// </summary>
    /// <remarks>
    /// This class has the same properties as Contact (or better: can have), but they
    /// are all nullable, and all have NullValueHandling.Ignore.
    /// </remarks>
    [CiviRequest]
    public partial class ContactRequest : BaseRequest
    {
        [JsonIgnore]
        public int? Id { get; set; }

        /// <summary>
        /// If Id is not set, IdValueExpression is passed to the API.
        /// Typical use case for chained call within a relationship
        /// call: "$value.contact_id_a".
        /// </summary>
        [JsonIgnore]
        public string IdValueExpression { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string IdString
        {
            get { return Id.HasValue ? Id.ToString() : IdValueExpression; }
        }

        [JsonProperty("external_identifier", NullValueHandling = NullValueHandling.Ignore)]
        public string ExternalIdentifier { get; set; }

        [JsonConverter(typeof (StringEnumConverter))]
        [JsonProperty("contact_type", NullValueHandling = NullValueHandling.Ignore)]
        public ContactType? ContactType { get; set; }

        [JsonProperty("contact_sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactSubType { get; set; }

        [JsonProperty("first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("middle_name", NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }

        [JsonProperty("last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("organization_name", NullValueHandling = NullValueHandling.Ignore)]
        public string OrganizationName { get; set; }

        [JsonProperty("gender_id", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof (NullableEnumConverter))]
        public Gender? Gender { get; set; }

        [JsonProperty("birth_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("is_deceased", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof (BoolConverter))]
        public bool IsDeceased { get; set; }

        [JsonProperty("deceased_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DeceasedDate { get; set; }

        [JsonProperty("preferred_mail_format", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof (StringEnumConverter))]
        public MailFormat? PreferredMailFormat { get; set; }

        #region chaining

        [JsonProperty("api.address.get", NullValueHandling = NullValueHandling.Ignore)]
        public AddressRequest AddressGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.address.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AddressRequest> AddressSaveRequest { get; set; }

        [JsonProperty("api.phone.get", NullValueHandling = NullValueHandling.Ignore)]
        public BaseRequest PhoneGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.phone.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseRequest> PhoneSaveRequest { get; set; }

        [JsonProperty("api.email.get", NullValueHandling = NullValueHandling.Ignore)]
        public BaseRequest EmailGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.email.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseRequest> EmailSaveRequest { get; set; }

        [JsonProperty("api.website.get", NullValueHandling = NullValueHandling.Ignore)]
        public BaseRequest WebsiteGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.website.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseRequest> WebsiteSaveRequest { get; set; }

        [JsonProperty("api.im.get", NullValueHandling = NullValueHandling.Ignore)]
        public BaseRequest ImGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.im.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseRequest> ImSaveRequest { get; set; }

        [JsonProperty("api.relationship.get", NullValueHandling = NullValueHandling.Ignore)]
        public RelationshipRequest RelationshipGetRequest { get; set; }

        [JsonConverter(typeof (Crm15815Converter))]
        [JsonProperty("api.relationship.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<RelationshipRequest> RelationshipSaveRequest { get; set; }

        [JsonProperty("api.membership.get", NullValueHandling = NullValueHandling.Ignore)]
        public BaseRequest MembershipGetRequest { get; set; }

        [JsonProperty("api.membership.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseRequest> MembershipSaveRequest { get; set; }
    
        #endregion

        #region CRM-15815
        // To lower the chance of hitting CRM-15815, don't pass empty create chains.
        public bool ShouldSerializeAddressSaveRequest()
        {
            return AddressSaveRequest != null && AddressSaveRequest.Any();
        }
        public bool ShouldSerializePhoneSaveRequest()
        {
            return PhoneSaveRequest != null && PhoneSaveRequest.Any();
        }
        public bool ShouldSerializeEmailSaveRequest()
        {
            return EmailSaveRequest != null && EmailSaveRequest.Any();
        }
        public bool ShouldSerializeWebsiteSaveRequest()
        {
            return WebsiteSaveRequest != null && WebsiteSaveRequest.Any();
        }
        public bool ShouldSerializeImSaveRequest()
        {
            return ImSaveRequest != null && ImSaveRequest.Any();
        }
        public bool ShouldSerializeRelationshipSaveRequest()
        {
            return RelationshipSaveRequest != null && RelationshipSaveRequest.Any();
        }
        #endregion
    }
}
