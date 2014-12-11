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
using System.Collections.Generic;

namespace Chiro.CiviCrm.Wcf.Example
{
    /// <summary>
    /// Example for the CiviCrm-API proof of concept
    /// </summary>
    /// <remarks>
    /// Please check your configuration!
    /// 
    /// The settings of Chiro.CiviCrm.Client should contain your CiviCRM site key
    /// and the key of your CiviCRM-API-user.
    /// 
    /// The url of the CiviCRM API is in the App.Config of this project.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new CiviCrmClient())
            {
                // This example works on the contact with contactId 2.
                const int contactId = 2;

                var contact = client.ContactGet(contactId);

                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                }
                else
                {
                    Console.WriteLine("Found: {0} {1} ({4}); id: {2}; {3}", contact.FirstName, contact.LastName, contact.Id, contact.ContactType, contact.GenderId);
                    Console.WriteLine("Birth date: {0}", contact.BirthDate);
                    Console.WriteLine("Deceased date: {0}", contact.DeceasedDate);
                    Console.WriteLine("External ID: {0}", contact.ExternalIdentifier);

                    //// Change first name:
                    //contact.FirstName = "Jean";
                    //client.ContactSave(contact);
                }

                ShowAddresses(client.ContactAddressesGet(contact.Id.Value));

                // Add an address. Delete it again.

                var newAddress = new Address
                {
                    ContactId = contact.Id.Value,
                    StreetAddress = "Hoefslagstraatje 2",
                    PostalCode = "9000",
                    City = "Gent",
                    Country = "BE",
                };

                newAddress = client.AddressSave(newAddress);
                ShowAddresses(client.ContactAddressesGet(contact.Id.Value));
                client.AddressDelete(newAddress.Id.Value);
                ShowAddresses(client.ContactAddressesGet(contact.Id.Value));
            }

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }

        private static void ShowAddresses(IEnumerable<Address> adresses)
        {
            Console.WriteLine("\nAddresses:");
            foreach (var a in adresses)
            {
                Console.WriteLine("  Address {0}: {1}, {2} {5} {3} - {4}", a.Id, a.StreetAddress, a.PostalCode, a.City, a.CountryId, a.PostalCodeSuffix);
            }
        }
    }
}
