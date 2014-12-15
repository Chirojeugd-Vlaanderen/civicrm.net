﻿/*
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
    /// A request for a contact based on its external ID.
    /// </summary>
    public class ExternalIdentifierRequest: BaseRequest
    {
        /// <summary>
        /// Constructs an external identifier request for the contact with given
        /// <paramref name="externalIdentifier"/>.
        /// </summary>
        /// <param name="externalIdentifier"></param>
        /// <remarks>If you want to do a chained call, just provide a value for
        /// the ChainedEntities property.</remarks>
        public ExternalIdentifierRequest(string externalIdentifier): base()
        {
            ExternalIdentifier = externalIdentifier;
        }

        /// <summary>
        /// Construct an empty external identifier request.
        /// </summary>
        public ExternalIdentifierRequest() : this(null) { }

        public string ExternalIdentifier { get; set; }
    }
}
