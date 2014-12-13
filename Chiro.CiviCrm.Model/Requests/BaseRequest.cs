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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chiro.CiviCrm.Model.Requests
{
    /// <summary>
    /// This class is used to send a get-request to the CiviCRM API. The base request
    /// will just return the first 25 entities. Use one of the inheriting requests
    /// for more fine grained search.
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// Fields to return, comma seperated.
        /// </summary>
        public string ReturnFields { get; set; }
        /// <summary>
        /// Entities to chain.
        /// </summary>
        public CiviEntity[] ChainedEntities { get; set; }
    }
}
