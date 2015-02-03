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
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviCrm.Wcf.Example.Properties;
using Newtonsoft.Json.Linq;

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
        private const string ExternalId = "100461";

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
   
            Example9();

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
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
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
        /// Gets a contact with the generic GetSingle.
        /// </summary>
        static void Example0_1()
        {
            using (var client = _factory.CreateChannel())
            {
                var result =
                    client.GetSingle(ApiKey, SiteKey, CiviEntity.Contact,
                        new ContactRequest {ExternalIdentifier = ExternalId}) as JObject;

                if (result == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }
                var contact = result.ToObject<Contact>();
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
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
                {
                    ExternalIdentifier = ExternalId,
                    AddressGetRequest = new BaseRequest()
                });

                // Keep the contact Id for later reference.
                int contactId = contact.Id;

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
                contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
                {
                    Id = contactId,
                    // We don't need all fields of the contact, we are only interested in the
                    // addresses.

                    // ReturnFields are still in civicrm notation, meaning lowercase and
                    // underscores (see issue #19)
                    ReturnFields = "id",
                    AddressGetRequest = new BaseRequest()
                });

                // Show adresses
                ShowAddresses(contact);

                // Delete the added addres
                client.AddressDelete(ApiKey, SiteKey, new IdRequest(newAddress.Id.Value));

                // Get the adresses again, to verify that the new address is gone.
                contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
                {
                    Id = contactId,
                    ReturnFields = "id",
                    AddressGetRequest = new BaseRequest()
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
                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
                    {
                        ExternalIdentifier = ExternalId,
                        AddressGetRequest = new BaseRequest()
                    });

                // Exit if contact is not found.
                if (contact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                // Keep the contact Id for later reference.
                int contactId = contact.Id;

                ShowContact(contact);

                // Change first name and birth date:
                contact.FirstName = "Jos";
                contact.BirthDate = new DateTime(1979, 3, 3);
                client.ContactSave(ApiKey, SiteKey,
                    new ContactRequest {Id = contactId, FirstName = "Jos", BirthDate = new DateTime(1979, 3, 3)});

                // Get contact again, to see whether it has worked.
                contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest
                {
                    ExternalIdentifier = ExternalId,
                    AddressGetRequest = new BaseRequest()
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
                var contactRequest = new ContactRequest
                {
                    ExternalIdentifier = ExternalId,
                    FirstName = "Wesley",
                    LastName = "Decabooter",
                    // use external ID to find the contact, instead of contact id.
                    ApiOptions = new ApiOptions{Match = "external_identifier"}
                };

                client.ContactSave(
                    ApiKey, SiteKey, contactRequest 
                    );

                // Get the contact again. First name and last name
                // should be updated. Other info should still be
                // there.

                var contact = client.ContactGetSingle(ApiKey, SiteKey, new ContactRequest {ExternalIdentifier = ExternalId});
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
                var contact = client.ContactGetSingle(ApiKey, SiteKey,
                    new ContactRequest {ExternalIdentifier = ExternalId});
                ShowContact(contact);

                contact.Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male;
                contact.PreferredMailFormat = contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML;

                var result = client.ContactSave(
                    ApiKey, SiteKey, new ContactRequest
                    {
                        Id = contact.Id,
                        Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male,
                        PreferredMailFormat =
                            contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML
                    }
                    );

                // Get contact again to check.

                contact = client.ContactGetSingle(ApiKey, SiteKey,
                    new ContactRequest {ExternalIdentifier = ExternalId});
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
                var contact = new ContactRequest
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
                var contactResult = client.ContactGet(ApiKey, SiteKey,
                    new ContactRequest {ExternalIdentifier = ExternalId});

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
                    new ContactRequest
                    {
                        ExternalIdentifier = ExternalId,
                        PhoneGetRequest = new BaseRequest(),
                        EmailGetRequest = new BaseRequest(),
                        ImGetRequest = new BaseRequest(),
                        WebsiteGetRequest = new BaseRequest(),
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
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        WebsiteSaveRequest = new List<BaseRequest> {new Website {Url = "http://smurf.com"}}
                    });
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(ApiKey, SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        WebsiteGetRequest = new BaseRequest()
                    });

                ShowContact(contact);
                ShowCommunication(contact);

                // Delete contact

                client.ContactDelete(ApiKey, SiteKey, new IdRequest(result.Id.Value), 1);
            }
        }

        /// <summary>
        /// Relationship example
        /// </summary>
        public static void Example9()
        {
            using (var client = _factory.CreateChannel())
            {
                var result = client.RelationshipGet(ApiKey, SiteKey, new RelationshipRequest {Id = 12388});
                var relationship = result.Values.FirstOrDefault();

                if (relationship == null)
                {
                    Console.WriteLine("Relationship not found.");
                }
                else
                {
                    Console.WriteLine("Contact ID a: {0}", relationship.ContactIdA);
                    Console.WriteLine("Contact ID b: {0}", relationship.ContactIdB);
                    Console.WriteLine("Start date: {0}", relationship.StartDate);
                    Console.WriteLine("End date: {0}", relationship.EndDate);
                }
            }
        }

        private static void ShowCommunication(Contact contact)
        {
            if (contact.PhoneResult != null && contact.PhoneResult.Count > 0)
            {
                foreach (var p in contact.PhoneResult.Values)
                {
                    Console.WriteLine("Phone ({0}): {1}", p.PhoneType, p.PhoneNumber);
                }
            }
            if (contact.EmailResult != null && contact.EmailResult.Count > 0)
            {
                foreach (var e in contact.EmailResult.Values)
                {
                    Console.WriteLine("E-mail ({0}): {1}", e.LocationTypeId, e.EmailAddress);
                }
            }
            if (contact.WebsiteResult != null && contact.WebsiteResult.Count > 0)
            {
                foreach (var w in contact.WebsiteResult.Values)
                {
                    Console.WriteLine("Website ({0}): {1}", w.WebsiteType, w.Url);
                }
            }
            if (contact.ImResult != null && contact.ImResult.Count > 0)
            {
                foreach (var im in contact.ImResult.Values)
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
            foreach (var a in c.AddressResult.Values)
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
