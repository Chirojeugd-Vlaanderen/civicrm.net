/*
   Copyright 2013-2015 Chirojeugd-Vlaanderen vzw

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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Wcf.Example.Properties;

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
        private const string ExternalId = "29";

        // Get API key and site key from configuration.
        private static readonly string ApiKey = Settings.Default.ApiKey;
        private static readonly string SiteKey = Settings.Default.SiteKey;

        // Channel factory

        private static ChannelFactory<ICiviCrmApi> _factory;

        /// <summary>
        /// Choose any example you like to run.
        /// </summary>
        /// <param name="arg"></param>
        static void Main(string[] arg)
        {
            // Just use any usable endpoint in the config file.
            _factory = new ChannelFactory<ICiviCrmApi>("*");
   
            Example8();

            _factory.Close();
            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }

        /// <summary>
        /// This example gets a contact, and shows its info.
        /// </summary>
        static void Example0()
        {
            using (var client = _factory.CreateChannel())
            {
                // Get the contact, and chain the contact's addresses.
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest
                {
                    ExternalIdentifier = ExternalId
                });

                // Exit if contact is not found.
                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                ShowContact(contact);
            }
        }

        /// <summary>
        /// This example gets a contact, adds an address, and deletes the
        /// address again.
        /// </summary>
        static void Example1()
        {
            using (var client = _factory.CreateChannel())
            {
                // Get the contact, and chain the contact's addresses.
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = ExternalId,
                        ChainedGet = new[] { CiviEntity.Address }
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
                    ContactId = contact.Id,
                    StreetAddress = "Hoefslagstraatje 2",
                    PostalCode = "9000",
                    City = "Gent",
                    Country = "BE",
                    LocationTypeId = 1,
                };

                var result = client.AddressSave(ApiKey, SiteKey, newAddress);
                newAddress.Id = result.Id;

                // Get contact again, to find out whether the address 
                // has been added.
                // Note that we now use the CiviCRM contact ID.
                contact = client.ContactGetSingle(ApiKey, SiteKey, new IdRequest
                {
                    Id = contactId,
                    // We don't need all fields of the contact, we are only interested in the
                    // addresses.

                    // ReturnFields are still in civicrm notation, meaning lowercase and
                    // underscores (see issue #19)
                    ReturnFields = "id",
                    ChainedGet = new[] { CiviEntity.Address }
                });

                // Show adresses
                ShowAddresses(contact);

                // Delete the added addres
                client.AddressDelete(ApiKey, SiteKey, new IdRequest(newAddress.Id.Value));

                // Get the adresses again, to verify that the new address is gone.
                contact = client.ContactGetSingle(ApiKey, SiteKey, new IdRequest
                {
                    Id = contactId,
                    ReturnFields = "id",
                    ChainedGet = new[] { CiviEntity.Address }
                });

                ShowAddresses(contact);
            }
        }

        /// <summary>
        /// This example changes name and birth date of a contact.
        /// </summary>
        public static void Example2()
        {
            using (var client = _factory.CreateChannel())
            {
                // Get the contact, and chain the contact's addresses.
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = ExternalId,
                        ChainedGet = new[] { CiviEntity.Address }
                    });

                // Exit if contact is not found.
                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                // Keep the contact Id for later reference.
                int contactId = contact.Id.Value;

                ShowContact(contact);

                // Change first name and birth date:
                contact.FirstName = "Jos";
                contact.BirthDate = new DateTime(1979, 3, 3);
                client.ContactSave(ApiKey, SiteKey, contact);

                // Get contact again, to see whether it has worked.
                contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest
                {
                    ExternalIdentifier = ExternalId,
                    ChainedGet = new[] { CiviEntity.Address }
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
            using (var client = _factory.CreateChannel())
            {
                var contact = new Contact
                {
                    ExternalIdentifier = ExternalId,
                    FirstName = "Wesley",
                    LastName = "Decabooter",
                    // use external ID to find the contact, instead of contact id.
                    ApiOptions = new ApiOptions{Match = "external_identifier"}
                };

                client.ContactSave(
                    ApiKey, SiteKey, contact 
                    );

                // Get the contact again. First name and last name
                // should be updated. Other info should still be
                // there.

                contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest(ExternalId));
                ShowContact(contact);
            }
        }

        /// <summary>
        ///  Changes gender and preferred mail format, as test for enums.
        /// </summary>
        public static void Example4()
        {
            using (var client = _factory.CreateChannel())
            {
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest(ExternalId));
                ShowContact(contact);

                contact.Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male;
                contact.PreferredMailFormat = contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML;
                
                var result = client.ContactSave(
                    ApiKey, SiteKey, contact
                    );

                // Get contact again to check.

                contact = client.ContactGetSingle(ApiKey, SiteKey, new ExternalIdentifierRequest(ExternalId));
                ShowContact(contact);
            }
        }

        /// <summary>
        /// Create a new contact with an external ID.
        /// </summary>
        public static void Example5()
        {
            using (var client = _factory.CreateChannel())
            {
                var contact = new Contact
                {
                    ContactType = ContactType.Individual,
                    FirstName = "Lucky",
                    LastName = "Luke",
                    BirthDate = new DateTime(1946, 3, 3),
                    Gender = Gender.Male,
                    ExternalIdentifier = "YADAYADA",
                    ApiOptions = new ApiOptions { Match = "external_identifier" }
                };
                var result = client.ContactSave(ApiKey, SiteKey, contact);

                ShowContact(result.Values.FirstOrDefault());

                client.ContactDelete(ApiKey, SiteKey, new IdRequest(result.Id.Value), 1);
            }
        }

        /// <summary>
        /// Website example.
        /// </summary>
        public static void Example6()
        {
            using (var client = _factory.CreateChannel())
            {
                var contactResult = client.ContactGet(ApiKey, SiteKey, new ExternalIdentifierRequest(ExternalId));

                var website = new Website
                {
                    ContactId = contactResult.Id,
                    Url = "http://blog.johanv.org",
                    WebsiteType = WebsiteType.Main
                };

                var result = client.WebsiteSave(ApiKey, SiteKey, website);
                website.Id = result.Values.First().Id;

                Console.WriteLine("Website added for contact with ContactID {0}  (external ID {1})", contactResult.Id, ExternalId);
                Console.WriteLine("You might want to check that. Then press enter.");
                Console.ReadLine();

                Debug.Assert(website.Id != null);
                client.WebsiteDelete(ApiKey, SiteKey, new IdRequest(website.Id.Value));
                
                Console.WriteLine("Website was deleted again.");
            }
        }

        /// <summary>
        /// Get a contact with all communication.
        /// </summary>
        public static void Example7()
        {
            using (var client = _factory.CreateChannel())
            {
                var contact = client.ContactGetSingle(ApiKey, SiteKey,
                    new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = ExternalId,
                        ChainedGet = new[] {CiviEntity.Phone, CiviEntity.Email, CiviEntity.Website, CiviEntity.Im}
                    });
                ShowContact(contact);
                ShowCommunication(contact);
            }
        }

        /// <summary>
        /// Chained writing
        /// </summary>
        public static void Example8()
        {
            using (var client = _factory.CreateChannel())
            {
                // Create a contact, chain website.
                var result = client.ContactSave(ApiKey, SiteKey,
                    new Contact
                    {
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ChainedCreate = new List<Website> {new Website {Url = "http://smurf.com"}}
                    });
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(ApiKey, SiteKey,
                    new IdRequest {Id = result.Id.Value, ChainedGet = new[] {CiviEntity.Website}});

                ShowContact(contact);
                ShowCommunication(contact);

                // Delete contact

                client.ContactDelete(ApiKey, SiteKey, new IdRequest(result.Id.Value), 1);
            }
        }

        private static void ShowCommunication(Contact contact)
        {
            if (contact.ChainedPhones != null && contact.ChainedPhones.Count > 0)
            {
                foreach (var p in contact.ChainedPhones.Values)
                {
                    Console.WriteLine("Phone ({0}): {1}", p.PhoneType, p.PhoneNumber);
                }
            }
            if (contact.ChainedEmails != null && contact.ChainedEmails.Count > 0)
            {
                foreach (var e in contact.ChainedEmails.Values)
                {
                    Console.WriteLine("E-mail ({0}): {1}", e.LocationTypeId, e.EmailAddress);
                }
            }
            if (contact.ChainedWebsites != null && contact.ChainedWebsites.Count > 0)
            {
                foreach (var w in contact.ChainedWebsites.Values)
                {
                    Console.WriteLine("Website ({0}): {1}", w.WebsiteType, w.Url);
                }
            }
            if (contact.ChainedIms != null && contact.ChainedIms.Count > 0)
            {
                foreach (var im in contact.ChainedIms.Values)
                {
                    Console.WriteLine("{0}: {1}", im.Provider, im.Name);
                }
            }
        }

        #region some output functions.

        private static void ShowContact(Contact contact)
        {
            // Show some information about the contact.
            Console.WriteLine("Found: {0} {1} ({4}); id: {2}; {3}", contact.FirstName, contact.LastName, contact.Id, contact.ContactType, contact.Gender);
            Console.WriteLine("Birth date: {0}", contact.BirthDate);
            Console.WriteLine("Deceased date: {0}", contact.DeceasedDate);
            Console.WriteLine("External ID: {0}", contact.ExternalIdentifier);
            Console.WriteLine("Mail Format: {0}", contact.PreferredMailFormat);
        }

        private static void ShowAddresses(Contact c)
        {
            Console.WriteLine("\nAddresses:");
            foreach (var a in c.ChainedAddresses.Values)
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
