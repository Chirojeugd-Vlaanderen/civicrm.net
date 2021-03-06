﻿/*
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
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Api.Converters
{
    /// <summary>
    /// Converter from nullable enum to char and vice versa.
    /// </summary>
    public class NullableEnumCharConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var underlyingType = Nullable.GetUnderlyingType(objectType);

            // Als het geen nullable is, is het tamelijk gemakkelijk:
            if (underlyingType == null)
            {
                return Enum.ToObject(objectType, (int) (Convert.ToChar(reader.Value)));
            }

            // Het is wel een nullable enum. Controleer dus eerst op null,
            // en converteer anders naar onderliggend type.

            return String.IsNullOrEmpty(reader.Value as string) ? null : Enum.ToObject(underlyingType, (int)(Convert.ToChar(reader.Value)));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((char)(int)value);
        }
    }
}
