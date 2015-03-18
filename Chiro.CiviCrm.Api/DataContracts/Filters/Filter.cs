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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Chiro.CiviCrm.Api.DataContracts.Filters
{
    public class Filter<T>: IFilter
    {
        public Filter()
        {
            Operator = WhereOperator.None;
        }
        public Filter(T value): this()
        {
            Values = new [] {value};
        }
        public Filter(WhereOperator op, params T[] values): this()
        {
            Operator = op;
            Values = values;
        }

        public WhereOperator Operator { get; set; }
        public T[] Values { get; private set; }

        IEnumerable<Object> IFilter.Objects
        {
            get { return Values.Cast<Object>(); }
        }
    }
}
