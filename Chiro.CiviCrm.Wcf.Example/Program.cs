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
            // Call the service to retrieve a contact based on its external identifier

            const int externalId = 300066;

            ICiviCrmClient client = new CiviCrmClient();
            // you could do this with dependency injection.

            var contact = client.ContactFind(externalId);
            // If it does not work, check your permissions

            if (contact == null)
            {
                Console.WriteLine("Contact not found.");
            }
            else
            {
                Console.WriteLine("Found: {0} {1}; id: {2}", contact.FirstName, contact.LastName, contact.Id); 
                Console.WriteLine("Date of birth: {0}", contact.BirthDate);

                // retrieve addresses

                foreach (var address in client.ContactAddressesFind(externalId))
                {
                    Console.WriteLine("{0}, {1} {2} {3}", address.StreetAddress, address.PostalCode, address.PostalCodeSuffix, address.City);
                }

                // you might want to do some modifications. You can uncomment the lines below;
                // that should work.

                //// change the name and birth date of the contact.

                //contact.FirstName = "Jos";
                //contact.BirthDate = new DateTime(1990, 4, 5);

                //client.ContactSave(contact);

                //// add address

                //client.AddressSave(new Address
                //                   {
                //                       Id = 0,
                //                       ContactId = contact.Id,
                //                       StreetAddress = "Kipdorp 130",
                //                       PostalCode = 2000,
                //                       City = "Antwerpen",
                //                       StateProvinceId = 1785,
                //                       CountryId = 1020,
                //                       LocationTypeId = 1
                //                   });
            }         

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }
    }
}
