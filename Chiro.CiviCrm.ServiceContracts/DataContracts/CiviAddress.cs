/*
   Copyright 2014 Chirojeugd-Vlaanderen vzw

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

using Chiro.CiviCrm.BehaviorExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chiro.CiviCrm.Api.DataContracts
{
    [DataContract]
    [JsonConvertible]
    public class CiviAddress
    {
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public int contact_id { get; set; }
        [DataMember]
        public int location_type_id { get; set; }
        [DataMember]
        public bool is_primary { get; set; }
        [DataMember]
        public bool is_billing { get; set; }
        [DataMember]
        public string street_address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string postal_code { get; set; }
        [DataMember]
        public int country_id { get; set; }
    }
}
