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
using Chiro.CiviCrm.Model;
using System.Collections.Generic;
using Chiro.CiviCrm.Model.Requests;

namespace Chiro.CiviCrm.Wcf.Example
{
    /// <summary>
    /// Examples for the CiviCrm-API proof of concept.
    /// </summary>
    /// <remarks>
    /// Please check the configuration in the App.config file of this project.
    /// You should adapt the endpoint address of your API, and your site key
    /// and api key in the settings.
    /// </remarks>
    class Program
    {
        // Put an existing external ID here:
        private const string externalId = "1111111";

        // Get API key and site key from configuration.
        private static readonly string _apiKey = Properties.Settings.Default.ApiKey;
        private static readonly string _siteKey = Properties.Settings.Default.SiteKey;

        /// <summary>
        /// Choose any example you like to run.
        /// </summary>
        /// <param name="arg"></param>
        static void Main(string[] arg)
        {
            Example3();

            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }


        /// <summary>
        /// This example gets a contact, adds an address, and deletes the
        /// address again.
        /// </summary>
        static void Example1()
        {
            using (var client = new CiviCrmClient(_siteKey, _apiKey))
            {
                // Get the contact, and chain the contact's addresses.
                var contact = client.ContactGetSingle(new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = externalId,
                        ChainedEntities = new[] { CiviEntity.Address }
                    });

                // Keep the contact Id for later reference.
                int contactId = contact.Id.Value;

                // Exit if contact is not found.
                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                ShowContact(contact);
                ShowAddresses(contact);

                // Add an address to the contact.
                var newAddress = new Address
                {
                    ContactId = contact.Id.Value,
                    StreetAddress = "Hoefslagstraatje 2",
                    PostalCode = "9000",
                    City = "Gent",
                    Country = "BE",
                };

                newAddress = client.AddressSave(newAddress, null);

                // Get contact again, to find out whether the address 
                // has been added.
                // Note that we now use the CiviCRM contact ID.
                contact = client.ContactGetSingle(new IdRequest
                {
                    Id = contactId,
                    // We don't need all fields of the contact, we are only interested in the
                    // addresses.

                    // ReturnFields are still in civicrm notation, meaning lowercase and
                    // underscores (see issue #19)
                    ReturnFields = "id",
                    ChainedEntities = new[] { CiviEntity.Address }
                });

                // Show adresses
                ShowAddresses(contact);

                // Delete the added addres
                client.AddressDelete(newAddress.Id.Value);

                // Get the adresses again, to verify that the new address is gone.
                contact = client.ContactGetSingle(new IdRequest
                {
                    Id = contactId,
                    ReturnFields = "id",
                    ChainedEntities = new[] { CiviEntity.Address }
                });

                ShowAddresses(contact);
            }
        }

        /// <summary>
        /// This example changes name and birth date of a contact.
        /// </summary>
        public static void Example2()
        {
            using (var client = new CiviCrmClient(_siteKey, _apiKey))
            {
                // Get the contact, and chain the contact's addresses.
                var contact = client.ContactGetSingle(new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = externalId,
                        ChainedEntities = new[] { CiviEntity.Address }
                    });

                // Keep the contact Id for later reference.
                int contactId = contact.Id.Value;

                // Exit if contact is not found.
                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                ShowContact(contact);

                // Change first name and birth date:
                contact.FirstName = "Jos";
                contact.BirthDate = new DateTime(1979, 3, 3);
                client.ContactSave(contact, null);

                // Get contact again, to see whether it has worked.
                contact = client.ContactGetSingle(new ExternalIdentifierRequest
                {
                    ExternalIdentifier = externalId,
                    ChainedEntities = new[] { CiviEntity.Address }
                });
                ShowContact(contact);
            }
        }

        /// <summary>
        /// This example updates a contact with a given external ID, without
        /// knowing the CiviCRM ID.
        /// </summary>
        public static void Example3()
        {
            using (var client = new CiviCrmClient(_siteKey, _apiKey))
            {
                var contact = new Contact
                {
                    ExternalIdentifier = externalId,
                    FirstName = "Wesley",
                    LastName = "Decabooter"
                };

                client.ContactSave(
                    contact, 
                    // use external ID to find the contact, instead of contact id.
                    new ApiOptions{Match = "external_identifier"});

                // Get the contact again. First name and last name
                // should be updated. Other info should still be
                // there.

                contact = client.ContactGetSingle(new ExternalIdentifierRequest(externalId));
                ShowContact(contact);
            }
        }

        #region some output functions.

        private static void ShowContact(Contact contact)
        {
            // Show some information about the contact.
            Console.WriteLine("Found: {0} {1} ({4}); id: {2}; {3}", contact.FirstName, contact.LastName, contact.Id, contact.ContactType, contact.GenderId);
            Console.WriteLine("Birth date: {0}", contact.BirthDate);
            Console.WriteLine("Deceased date: {0}", contact.DeceasedDate);
            Console.WriteLine("External ID: {0}", contact.ExternalIdentifier);
        }

        private static void ShowAddresses(Contact c)
        {
            Console.WriteLine("\nAddresses:");
            foreach (var a in c.ChainedAddresses)
            {
                Console.WriteLine(
                    "  Address {0}: {1}, {2} {5} {3} - {4},{6}", 
                    a.Id, 
                    a.StreetAddress, 
                    a.PostalCode, 
                    a.City, 
                    a.CountryId, 
                    a.PostalCodeSuffix,
                    a.StateProvinceId);
            }
        }

        #endregion
    }
}
