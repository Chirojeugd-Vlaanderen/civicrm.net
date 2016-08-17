﻿/*
   Copyright 2015, 2016 Chirojeugd-Vlaanderen vzw

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
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.Converters
{
    /// <summary>
    /// DateTime? converter to work around CRM-15863
    /// https://issues.civicrm.org/jira/browse/CRM-15863
    /// </summary>
    public class Crm15863Converter: JsonConverter
    {
        public static Regex LotsOfDigitsExpression = new Regex("([0-9]{4})([0-9]{2})([0-9]{2})(([0-9]{2})([0-9]{2})([0-9]{2}))?");

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? datetime = (DateTime?) value;

            // As a workaround for #92 we will remove the existing datetime if DateTime.MinValue is given.
            // (Hmmm, this doesn' t have anything to do with CRM-15863)
            if (datetime == DateTime.MinValue)
            {
                // The CiviCRM API will interpret this as a NULL value for a datetime.
                writer.WriteValue(0);
                return;
            }
            writer.WriteValue((DateTime?) value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string input = reader.Value as string;
            DateTime result;

            if (input == null)
            {
                return null;
            }
            if (DateTime.TryParse(input, out result))
            {
                return result;
            }
            if (!LotsOfDigitsExpression.IsMatch(input))
            {
                return null;
            }
            var parts = LotsOfDigitsExpression.Match(input).Groups;

            if (!String.IsNullOrEmpty(parts[6].ToString()))
            {
                // If there are seconds, create date and time
                return new DateTime(int.Parse(parts[1].Value), int.Parse(parts[2].Value), int.Parse(parts[3].Value),
                    int.Parse(parts[5].Value), int.Parse(parts[6].Value), int.Parse(parts[7].Value));
            }
            // Only a date.
            return new DateTime(int.Parse(parts[1].Value), int.Parse(parts[2].Value), int.Parse(parts[3].Value));
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime?);
        }
    }
}
