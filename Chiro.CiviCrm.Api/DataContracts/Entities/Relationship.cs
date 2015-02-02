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
    [DataContract]
    public partial class Relationship
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }

        [DataMember(Name="contact_id_a")]
        public int ContactIdA { get; set; }

        [DataMember(Name="contact_id_b")]
        public int ContactIdB { get; set; }

        [DataMember(Name="relationship_type_id")]
        public int RelationshipTypeId { get; set; }

        [DataMember(Name = "start_date")]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? StartDate { get; set; }

        [DataMember(Name = "end_date")]
        [JsonConverter(typeof(Crm15863Converter))]
        public DateTime? EndDate { get; set; }

        [DataMember(Name = "is_active")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsActive { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "is_permission_a_b")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsPermissionAb { get; set; }

        [DataMember(Name = "is_permission_b_a")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsPermissionBa { get; set; }
    }
}
