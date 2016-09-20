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
using System.Diagnostics;
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.Converters
{
    public class FilterConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var filter = value as IFilter;
            string civiOperator;
            string rhs;

            Debug.Assert(filter != null);

            var first = filter.Objects.FirstOrDefault();
            if (first == null)
            {
                if (filter.Operator != WhereOperator.IsNotNull && filter.Operator != WhereOperator.IsNull)
                {
                    // no RHS only possible for is (not) null.
                    throw new InvalidOperationException();
                }
                rhs = String.Empty;
            }
            else if (filter.Objects.Skip(1).FirstOrDefault() == null)
            {
                // Only one object. Easy.
                Object obj = filter.Objects.First();
                if (obj is DateTime && ((DateTime) obj) == DateTime.MinValue)
                {
                    // Work around #92. If you use this in a create-operation, you can remove the existing
                    // datetime by passing DateTime.MinValue.
                    obj = 0;
                }

                rhs = JsonConvert.SerializeObject(obj);
            }
            else
            {
                // More than one object. Check for (not) in and (not) between.
                if (filter.Operator == WhereOperator.Between || filter.Operator == WhereOperator.NotBetween)
                {
                    if (filter.Objects.Skip(2).FirstOrDefault() != null)
                    {
                        // More than 2 objects does not make sense for between.
                        throw new InvalidOperationException();
                    }
                }
                else if (filter.Operator != WhereOperator.In && filter.Operator != WhereOperator.NotIn)
                {
                    throw new InvalidOperationException();
                }
                rhs = JsonConvert.SerializeObject(filter.Objects);
            }

            switch (filter.Operator)
            {
                case WhereOperator.None:
                    writer.WriteRawValue(rhs);
                    return;
                case WhereOperator.Eq:
                    civiOperator = "=";
                    break;
                case WhereOperator.Lte:
                    civiOperator = "<=";
                    break;
                case WhereOperator.Gte:
                    civiOperator = ">=";
                    break;
                case WhereOperator.Lt:
                    civiOperator = "<";
                    break;
                case WhereOperator.Gt:
                    civiOperator = ">";
                    break;
                case WhereOperator.Like:
                    civiOperator = "LIKE";
                    break;
                case WhereOperator.Ne:
                    civiOperator = "<>";
                    break;
                case WhereOperator.NotLike:
                    civiOperator = "NOT LIKE";
                    break;
                case WhereOperator.In:
                    civiOperator = "IN";
                    break;
                case WhereOperator.NotIn:
                    civiOperator = "NOT IN";
                    break;
                case WhereOperator.Between:
                    civiOperator = "BETWEEN";
                    break;
                case WhereOperator.NotBetween:
                    civiOperator = "NOT BETWEEN";
                    break;
                case WhereOperator.IsNotNull:
                    writer.WriteRawValue("{\"IS NOT NULL\":1}");
                    return;
                case WhereOperator.IsNull:
                    writer.WriteRawValue("{\"IS NULL\":1}");
                    return;
                default:
                    throw new InvalidOperationException();
            }
            writer.WriteRawValue(String.Format("{{\"{0}\":{1}}}", civiOperator, rhs));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // We only use this converter for requests, so
            // we'll never have to deserialize such a thing.
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType.IsGenericType &&
                    objectType.GetGenericTypeDefinition() == typeof (Filter<>));
        }
    }
}
