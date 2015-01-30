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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chiro.CiviCrm.Api.Converters
{
    /// <summary>
    /// Request-to-JSON converter to decrease the chance on hitting
    /// upstream CiviCRM issue #15815. Don't pass arrays to a chained
    /// call that only have one element. In that case just pass the
    /// element.
    /// </summary>
    public class Crm15815Converter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var things = value as IEnumerable<Object>;

            // It does not matter that things might be enumerated multiple times,
            // we only look at the first and the second item.

            if (things != null && things.FirstOrDefault() != null && things.Skip(1).FirstOrDefault() == null)
            {
                // Exactly 1 object => pass object.
                serializer.Serialize(writer, things.First());
            }
            else
            {
                serializer.Serialize(writer, things);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (IEnumerable<Object>).IsAssignableFrom(objectType);
        }
    }
}
