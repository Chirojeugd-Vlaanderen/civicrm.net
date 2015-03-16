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
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.Converters
{
    public class FilterConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var source = value as IFilter;
            string civiOperator;

            Debug.Assert(source != null);
            switch (source.Operator)
            {
                case WhereOperator.None:
                    writer.WriteValue(source.Object);
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
                    // We cannot do this (yet), because
                    // we have only one value for the moment.
                    throw new NotSupportedException();
                case WhereOperator.NotIn:
                    throw new NotSupportedException();
                case WhereOperator.Between:
                    throw new NotSupportedException();
                case WhereOperator.NotBetween:
                    throw new NotSupportedException();
                case WhereOperator.IsNotNull:
                    writer.WriteRawValue("{{\"IS NOT NULL\":1}}");
                    return;
                case WhereOperator.IsNull:
                    writer.WriteRawValue("{{\"IS NULL\":1}}");
                    return;
                default:
                    throw new InvalidOperationException();
            }
            writer.WriteRawValue(String.Format("{{\"{0}\":{1}}}", civiOperator, JsonConvert.SerializeObject(source.Object)));
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
