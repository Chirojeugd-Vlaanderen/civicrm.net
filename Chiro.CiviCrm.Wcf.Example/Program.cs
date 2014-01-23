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

            ICiviCrmClient client = new CiviCrmClient();
            // you could do this with dependency injection.

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
                Console.WriteLine("Found: {0} {1}; id: {2}", contact.FirstName, contact.LastName, contact.Id); 
                Console.WriteLine("Date of birth: {0}", contact.BirthDate);
                Console.WriteLine("Gender: {0}", contact.Gender);
                Console.WriteLine("External ID: {0}", contact.ExternalId);

                // retrieve addresses

                foreach (var address in client.ContactAddressesGet(contactId))
                {
                    Console.WriteLine(
                        "(7) {0}, {1} {2} {3} - Country-ID {4} P:{5} B:{6}", 
                        address.StreetAddress, 
                        address.PostalCode, 
                        address.PostalCodeSuffix, 
                        address.City, 
                        address.Country,
                        address.IsPrimary,
                        address.IsBilling,
                        address.Id
                        );
                }

                // you might want to do some modifications. You can uncomment the lines below;
                // that should work.

                //// change the name, gender and birth date of the contact.

                //contact.FirstName = "Jos";
                //contact.BirthDate = new DateTime(1990, 4, 5);
                //contact.Gender = Gender.Female;

                //// add address

                //client.AddressSave(new Address
                //                   {
                //                       Id = 0,
                //                       ContactId = contact.Id,
                //                       StreetAddress = "Kipdorp 130",
                //                       PostalCode = 2000,
                //                       City = "Antwerpen",
                //                       StateProvinceId = 1785,
                //                       Country = "BE",
                //                       LocationTypeId = 1,
                //                       IsPrimary = true,
                //                       IsBilling = true,
                //                   });
            }         

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }
    }
}
