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

using System.Runtime.Serialization;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// Chirocivi custom fields for event.
    /// </summary>
    public partial class Event
    {
        [DataMember(Name = "custom_50")]
        public string KipId { get; set; }

        [DataMember(Name = "custom_51")]
        public string BronBoekhouding { get; set; }

        [DataMember(Name = "custom_52")]
        public string AnalytischeCode { get; set; }

        [DataMember(Name = "custom_53")]
        public int? OrganiserendePersoon1Id { get; set; }

        [DataMember(Name = "custom_54")]
        public int? OrganiserendePersoon2Id { get; set; }

        [DataMember(Name = "custom_55")]
        public int? OrganiserendePersoon3Id { get; set; }

        [DataMember(Name = "custom_56")]
        public int? OrganiserendePloeg1Id { get; set; }

        [DataMember(Name = "custom_57")]
        public int? OrganiserendePloeg2Id { get; set; }

        [DataMember(Name = "custom_58")]
        public int? OrganiserendePloeg3Id { get; set; }

        [DataMember(Name = "custom_59")]
        public int? AantalVormingsUren { get; set; }

        [DataMember(Name = "custom_60")]
        public string OfficieelCursusNummer { get; set; }

        [DataMember(Name = "custom_61")]
        public int? GapUitstapId { get; set; }
    }
}