﻿/*
   Copyright 2013 Chirojeugd-Vlaanderen vzw

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

namespace Chiro.CiviCrm.ServiceContracts.DataContracts
{
    [DataContract]
    public enum ContactType
    {
        [EnumMember] 
        Individual = 1,
        [EnumMember]
        Organization = 2,
        [EnumMember]
        Household = 3
    }

    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Female = 1,
        [EnumMember]
        Male = 2,
        [EnumMember]
        Transgender = 3
    }
}
