/*
   Copyright 2013 Chirojeugd-Vlaanderen vzw

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
using System.ServiceModel.Dispatcher;

namespace Chiro.CiviCrm.BehaviorExtension
{
    /// <summary>
    /// QueryStringConverter for nullable types.
    /// Thank you, Xcalibur: http://stackoverflow.com/users/317739/xcalibur
    /// </summary>
    public class NullableQueryStringConverter: QueryStringConverter
    {
        public override bool CanConvert(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            return (underlyingType != null && base.CanConvert(underlyingType)) || base.CanConvert(type);
        }

        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            var underlyingType = Nullable.GetUnderlyingType(parameterType);

            // Handle nullable types
            if (underlyingType != null)
            {
                // Define a null value as being an empty or missing (null) string passed as the query parameter value
                return String.IsNullOrEmpty(parameter) ? null : base.ConvertStringToValue(parameter, underlyingType);
            }

            return base.ConvertStringToValue(parameter, parameterType);
        }

        public override string ConvertValueToString(object parameter, Type parameterType)
        {
            var underlyingType = Nullable.GetUnderlyingType(parameterType);

            if (underlyingType != null)
            {
                return base.ConvertValueToString(parameter, underlyingType);
            }

            return base.ConvertValueToString(parameter, parameterType);
        }
    }
}
