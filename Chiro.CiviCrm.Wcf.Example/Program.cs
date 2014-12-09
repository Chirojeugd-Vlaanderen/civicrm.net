﻿/*
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
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new CiviCrmClient())
            {
                var contact = client.ContactFind("1111111");
                //var contact = client.ContactGet(11890);
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
                    Console.WriteLine("External ID: {0}", contact.ExternalIdentifier);

                    contact.FirstName = "Donny";
                    client.ContactSave(contact);
                }

                ShowAddresses(client.ContactAddressesGet(contact.Id.Value));

                var newAddress = new Address
                {
                    ContactId = contact.Id.Value,
                    StreetAddress = "Hoefslagstraatje 2",
                    PostalCode = "9000",
                    City = "Gent",
                    CountryId = 1020
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
                Console.WriteLine("  Address {0}: {1}, {2} {3}", a.Id, a.StreetAddress, a.PostalCode, a.City);
            }
        }
    }
}
