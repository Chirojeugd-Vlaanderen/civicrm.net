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
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;

namespace Chiro.CiviCrm.Api.DataContracts.Entities
{
    /// <summary>
    /// A CiviCRM LocBlock.
    /// </summary>
    /// <remarks>
    /// I did not invent this :-)
    /// </remarks>
    [DataContract]
    public partial class LocBlock
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "address_id")]
        public int? AddressId { get; set; }
        [DataMember(Name = "email_id")]
        public int? EmailId { get; set; }
        [DataMember(Name = "phone_id")]
        public int? PhoneId { get; set; }
        [DataMember(Name = "im_id")]
        public int? ImId { get; set; }
        [DataMember(Name = "address_2_id")]
        public int? Address2Id { get; set; }
        [DataMember(Name = "email_2_id")]
        public int? Email2Id { get; set; }
        [DataMember(Name = "phone_2_id")]
        public int? Phone2Id { get; set; }
        [DataMember(Name = "im_2_id")]
        public int? Im2Id { get; set; }

        #region Chaining
        [DataMember(Name = "api.Address.get")]
        public ApiResultValues<Address>  AddressResult { get; set; }

        [DataMember(Name = "api.Event.get")]
        public ApiResultValues<Event> EventResult { get; set; }
        #endregion
    }
}
