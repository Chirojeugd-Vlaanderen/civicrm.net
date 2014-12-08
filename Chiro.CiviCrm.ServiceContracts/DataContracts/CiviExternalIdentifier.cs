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

using Chiro.CiviCrm.BehaviorExtension;
using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Some class that basically converts an external identifier to the json-part of the request url.
    /// </summary>
    [JsonConvertible]
    public class CiviExternalIdentifier
    {
        public string external_identifier { get; set; }

        public CiviExternalIdentifier(string externalIdentifier)
        {
            external_identifier = externalIdentifier;
        }

        public override string ToString()
        {
            return String.Format(
                "{{\"sequential\":1, \"external_identifier\":{0}}}", 
                Microsoft.Security.Application.Encoder.JavaScriptEncode(external_identifier, false));
        }
    }
}
