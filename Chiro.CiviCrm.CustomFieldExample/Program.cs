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

using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.CustomFieldExample.Properties;

namespace Chiro.CiviCrm.CustomFieldExample
{
    /// <summary>
    /// This program illustrates how you can address your custom fields in CiviCRM.
    /// 
    /// If you are looking for a more general example, please see the project
    /// Chiro.CiviCrm.Wcf.Example.
    /// </summary>
    class Program
    {
        private static readonly string _siteKey = Settings.Default.SiteKey;
        private static readonly string _apiKey = Settings.Default.ApiKey;

        static void Main(string[] args)
        {
            var customContact = new CustomContact
            {
                ContactType = ContactType.Individual,
                Gender = Gender.Male,
                FirstName = "Will",
                LastName = "Tura",
                GapId = 797204
            };

            using (var channelFactory = new ChannelFactory<ICiviCrmApi>("*"))
            {
                using (var channel = channelFactory.CreateChannel())
                {
                    channel.ContactSave(_apiKey, _siteKey, customContact);
                }
            }

        }
    }
}
