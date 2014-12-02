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
using Chiro.CiviCrm.Client;
using Chiro.CiviCrm.ClientInterfaces;
using Chiro.CiviCrm.Domain;

namespace Chiro.CiviCrm.Wcf.Example
{
    /// <summary>
    /// Example for the CiviCrm-API proof of concept
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Call the service to retrieve a contact based on its Civi-ID

            const int contactId = 5;

            using (var client = new CiviCrmClient())
            {
                var contact = client.ContactGet(contactId);
                // If you want to access the CiviCRM-API over https (recommended), you should
                // change the security mode of the CiviCrmBindingConfiguration from None to Transport
                // (in App.config)
                // If it still does not work, check your permissions

                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                }
                else
                {
                    Console.WriteLine("Found: {0} {1}; id: {2}", contact.first_name, contact.last_name, contact.contact_id);
                    Console.WriteLine("External ID: {0}", contact.external_identifier);
                }
            }

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }
    }
}
