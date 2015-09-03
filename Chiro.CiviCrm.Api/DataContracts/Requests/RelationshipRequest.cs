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
using Chiro.CiviCrm.Api.Converters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    [CiviRequest]
    public partial class RelationshipRequest: BaseRequest
    {
        /// <summary>
        /// If ContactIdA is set, ContactIdA will be passed to the API.
        /// </summary>
        [JsonIgnore]
        public int? ContactIdA { get; set; }

        /// <summary>
        /// If ContactIdA is not set, ContactIdAValueExpression is passed to the API.
        /// Typical use case: "$value.id".
        /// </summary>
        [JsonIgnore]
        public string ContactIdAValueExpression { get; set; }

        /// <summary>
        /// This property is read-only. It decides whether ContactIdA or ContactIdAValueExpression
        /// is passed to the API.
        /// </summary>
        [JsonProperty("contact_id_a", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactIdAString { get
        {
            return ContactIdA.HasValue ? ContactIdA.ToString() : ContactIdAValueExpression;
        }}

        /// <summary>
        /// If ContactIdB is set, ContactIdB will be passed to the API.
        /// </summary>
        [JsonIgnore]
        public int? ContactIdB { get; set; }

        /// <summary>
        /// If ContactIdB is not set, ContactIdBValueExpression is passed to the API.
        /// Typical use case: "$value.id".
        /// </summary>
        [JsonIgnore]
        public string ContactIdBValueExpression { get; set; }

        /// <summary>
        /// This property is read-only. It decides whether ContactIdB or ContactIdBValueExpression
        /// is passed to the API.
        /// </summary>
        [JsonProperty("contact_id_b", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactIdBString
        {
            get
            {
                return ContactIdB.HasValue ? ContactIdB.ToString() : ContactIdBValueExpression;
            }
        }

        [JsonProperty("relationship_type_id", NullValueHandling = NullValueHandling.Ignore)]
        public int? RelationshipTypeId { get; set; }

        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? StartDate { get; set; }

        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? EndDate { get; set; }

        [JsonProperty("is_active", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? IsActive { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("is_permission_a_b", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? IsPermissionAb { get; set; }

        [JsonProperty("is_permission_b_a", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(BoolConverter))]
        public bool? IsPermissionBa { get; set; }

        #region chaining
        [JsonProperty("api.Contact.get", NullValueHandling = NullValueHandling.Ignore)]
        public ContactRequest ContactGetRequest { get; set; }

        [JsonConverter(typeof(Crm15815Converter))]
        [JsonProperty("api.Contact.create", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ContactRequest> ContactSaveRequest { get; set; }
        #endregion

        public override CiviEntity EntityType
        {
            get { return CiviEntity.Relationship; }
        }
    }
}
