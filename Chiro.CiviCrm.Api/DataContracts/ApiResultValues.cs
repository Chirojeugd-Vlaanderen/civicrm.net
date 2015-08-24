/*
   Copyright 2014, 2015 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.DataContracts.Entities;

namespace Chiro.CiviCrm.Api.DataContracts
{
    [DataContract]
    public class ApiResultValues<T>: ApiResult where T:IEntity
    {
        [DataMember(Name="values")]
        public T[] Values { get; set; }

        public ApiResultValues() : base()
        {
        }

        public ApiResultValues(T[] values) : this()
        {
            Values = values;
            Count = values.Length;
            if (Count == 1)
            {
                Id = values[0].Id;
            }
        }

        public ApiResultValues(T value) : this()
        {
            Values = new[] {value};
            Count = 1;
            Id = value.Id;
        }
    }
}
