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

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// Chirocivi custom fields for event.
    /// </summary>
    public partial class Event
    {
        [DataMember(Name = "custom_41")]
        public string KipId { get; set; }

        [DataMember(Name = "custom_43")]
        public string AnalytischeCode { get; set; }

        [DataMember(Name = "custom_44_id")]
        public int? OrganiserendePersoon1Id { get; set; }

        [DataMember(Name = "custom_45_id")]
        public int? OrganiserendePersoon2Id { get; set; }

        [DataMember(Name = "custom_46_id")]
        public int? OrganiserendePersoon3Id { get; set; }

        [DataMember(Name = "custom_47_id")]
        public int? OrganiserendePloeg1Id { get; set; }

        [DataMember(Name = "custom_48_id")]
        public int? OrganiserendePloeg2Id { get; set; }

        [DataMember(Name = "custom_49_id")]
        public int? OrganiserendePloeg3Id { get; set; }

        [DataMember(Name = "custom_50")]
        public int? AantalVormingsUren { get; set; }

        [DataMember(Name = "custom_51")]
        public string OfficieelCursusNummer { get; set; }

        [DataMember(Name = "custom_52")]
        public int? GapUitstapId { get; set; }
    }
}
