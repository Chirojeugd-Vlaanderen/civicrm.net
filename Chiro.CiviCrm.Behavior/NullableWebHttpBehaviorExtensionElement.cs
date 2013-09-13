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
using System.ServiceModel.Configuration;

namespace Chiro.CiviCrm.BehaviorExtension
{
    public class NullableWebHttpBehaviorExtensionElement: BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new NullableWebHttpBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(NullableWebHttpBehavior); }
        }
    }
}
