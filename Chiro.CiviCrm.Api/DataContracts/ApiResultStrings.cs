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

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Eenvoudig datacontract dat we gebruiken als de API veel informatie moet
    /// opleveren. Die informatie is dan een array, met als elementen opnieuw een array,
    /// die dan telkens 1 string bevat.
    /// 
    /// Dat is wat omslachtig, maar dat was zo tameklijk makkelijk te implementeren
    /// aan Civi-kant.
    /// </summary>
    [DataContract]
    public class ApiResultStrings: ApiResult
    {
        [DataMember(Name = "values")]
        public string[][] Values { get; set; }
    }
}
