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
using Chiro.Cdf.ServiceHelper;
using Chiro.CiviCrm.ServiceContracts;
using Chiro.CiviCrm.ServiceContracts.DataContracts;

namespace Chiro.CiviCrm.Wcf.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Call the service to retrieve a contact base on its external ID

            const int externalId = 300066;

            // The construction with ServiceHelper avoids that I have to create
            // a proxy myself. Alternatively, you could inherit one from
            // ClientBase<ICiviCrmApi>

            var result =
                ServiceHelper.CallService<ICiviCrmApi, ContactSet>(
                    svc =>
                        svc.ContactFind(Properties.Settings.Default.UserKey, Properties.Settings.Default.SiteKey,
                            externalId));

            Console.WriteLine(result.Contacts[0].FirstName);

            Console.ReadLine();
        }
    }
}
