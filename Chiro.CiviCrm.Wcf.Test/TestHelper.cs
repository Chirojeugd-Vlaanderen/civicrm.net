/*
   Copyright 2015 Johan Vervloet

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

using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Wcf.Test.Properties;

namespace Chiro.CiviCrm.Wcf.Test
{
    public static class TestHelper
    {
        // It is probably not recommended to keep one channel factory around for a long time.
        // But I guess it's OK for the unit tests.
        
        private static readonly ChannelFactory<ICiviCrmApi> Factory = new ChannelFactory<ICiviCrmApi>("*");

        public static string ApiKey
        {
            get { return Settings.Default.ApiKey; }
        }

        public static string SiteKey
        {
            get { return Settings.Default.SiteKey; }
        }

        public static ICiviCrmApi ClientGet()
        {
            return Factory.CreateChannel();
        }
    }
}
