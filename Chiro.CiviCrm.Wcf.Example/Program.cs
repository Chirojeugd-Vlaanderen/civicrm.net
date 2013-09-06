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
using Chiro.CiviCrm.Client;
using Chiro.CiviCrm.ClientInterfaces;

namespace Chiro.CiviCrm.Wcf.Example
{
    /// <summary>
    /// Example for the CiviCrm-API proof of concept
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Call the service to retrieve a contact base on its external ID

            const int externalId = 300066;

            ICiviCrmClient client = new CiviCrmClient(Properties.Settings.Default.UserKey, Properties.Settings.Default.SiteKey);
            // you could do this with dependency injection.

            var contact = client.ContactFind(externalId);

            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
            }
            else
            {
                Console.WriteLine("Found: {0} {1}", contact.FirstName, contact.LastName); 
   
                // change the name of the contact.

                contact.FirstName = "Jos";
                client.ContactSave(contact);
            }         

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }
    }
}
