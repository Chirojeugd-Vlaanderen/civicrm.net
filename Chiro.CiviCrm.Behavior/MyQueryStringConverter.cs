/*
   Copyright 2013, 2014 Chirojeugd-Vlaanderen vzw

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
    /// QueryStringConverter that uses 'ToString' when it cannot convert.
    /// </summary>
    public class MyQueryStringConverter: QueryStringConverter
    {
        public override bool CanConvert(Type type)
        {
            // I will convert anything :-)
            return true;
        }

        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            return base.CanConvert(parameterType) ? base.ConvertStringToValue(parameter, parameterType) : null;
        }

        public override string ConvertValueToString(object parameter, Type parameterType)
        {
            return base.CanConvert(parameterType) ? base.ConvertValueToString(parameter, parameterType) : parameter.ToString();
        }
    }
}
