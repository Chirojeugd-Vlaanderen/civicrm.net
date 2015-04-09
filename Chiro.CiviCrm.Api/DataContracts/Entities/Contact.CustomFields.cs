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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    // If you want to use custom fields of contacts, please add them here.
    // There is an example in the comments.
    // You could inherit from Contact as well, this would make retrieving the
    // custom fields a lot harder. And retrieving them in a chained call, would
    // be almost impossible.
    public partial class Contact
    {
        /// <summary>
        /// Bind the member 'GapId' to the correct custom ID.
        /// </summary>
        [DataMember(Name = "custom_1"), JsonProperty]
        public int? GapId { get; set; }
    }
}
