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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.DataContracts.Requests
{
    // If you want to use custom fields of contacts, please add them here.
    // There is an example in the comments.
    // You could inherit from ContactRequest as well.
    public partial class ContactRequest
    {
        ///// <summary>
        ///// Bind the member 'GapId' to the custom field custom_10.
        ///// </summary>
        //[JsonProperty("custom_10", NullValueHandling = NullValueHandling.Ignore)]
        //public int? GapId { get; set; }
    }
}
