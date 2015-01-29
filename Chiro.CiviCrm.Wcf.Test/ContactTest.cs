﻿/*
   Copyright 2014 Johan Vervloet

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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class ContactTest
    {
        private Contact _myContact;
        private Address _myAddress;

        private Phone _myPhone;
        private Email _myEmail;
        private Website _myWebsite;
        private Im _myIm;

        private int _myContactId;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        FirstName = "Joe",
                        LastName = "Schmoe",
                        ExternalIdentifier = "Unit_Test_External_ID",
                        // If the contact with given external identifier already exists,
                        // reuse it.
                        ApiOptions = new ApiOptions {Match = "external_identifier"}
                    });

                _myContact = result.Values.First();
                Debug.Assert(_myContact.Id.HasValue);
                _myContactId = _myContact.Id.Value;

                // TODO: chain this address creation.
                // (As soon as write chaining is supported.)
                var address = new Address
                {
                    ContactId = _myContact.Id,
                    StreetAddress = "Kipdorp 30",
                    PostalCode = "2000",
                    City = "Antwerpen",
                    CountryId = 1020,   // Belgium
                    LocationTypeId = 1,
                };
                var addressResult = client.AddressSave(TestHelper.ApiKey, TestHelper.SiteKey, address);
                _myAddress = addressResult.Values.First();

                _myPhone =
                    client.PhoneSave(TestHelper.ApiKey, TestHelper.SiteKey,
                        new Phone {ContactId = _myContact.Id, PhoneNumber = "02-345 67 89", PhoneType = PhoneType.Phone})
                        .Values.First();

                _myEmail =
                    client.EmailSave(TestHelper.ApiKey, TestHelper.SiteKey,
                        new Email {ContactId = _myContact.Id, EmailAddress = "joe@schmoe.com"})
                        .Values.First();

                _myWebsite =
                    client.WebsiteSave(TestHelper.ApiKey, TestHelper.SiteKey,
                        new Website
                        {
                            ContactId = _myContact.Id,
                            Url = "https://twitter.com/jschmoe",
                            WebsiteType = WebsiteType.Twitter
                        })
                        .Values.First();

                _myIm =
                    client.ImSave(TestHelper.ApiKey, TestHelper.SiteKey,
                        new Im {ContactId = _myContact.Id, Name = "joe.schmoe@facebook.com", Provider = Provider.Jabber})
                        .Values.First();
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myContact.Id ?? 0),
                    1);

                Debug.Assert(result.IsError == 0,
                    "Could not delete contact. Check for the delete_contact permission of your API user.");
            }
        }

        [TestMethod]
        public void ChainedAddressGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, new ContactRequest
                {
                    Id = _myContactId,
                    // We don't need all fields of the contact, we are only interested in the
                    // addresses.

                    // ReturnFields are still in civicrm notation, meaning lowercase and
                    // underscores (see issue #19)
                    ReturnFields = "id",
                    AddressGetRequest =  new BaseRequest()
                });
                Assert.IsTrue(contact.AddressResult.Values.Any(adr => adr.Id == _myAddress.Id));
            }
        }

        [TestMethod]
        public void ChainedCommunicationGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ExternalIdentifier = _myContact.ExternalIdentifier,
                        PhoneGetRequest = new BaseRequest(),
                        EmailGetRequest = new BaseRequest(),
                        WebsiteGetRequest = new BaseRequest(),
                        ImGetRequest = new BaseRequest()
                    });
                //new[] {CiviEntity.Phone, CiviEntity.Email, CiviEntity.Website, CiviEntity.Im}
                Assert.IsTrue(contact.PhoneResult.Values.Any(src => src.Id == _myPhone.Id));
                Assert.IsTrue(contact.EmailResult.Values.Any(src => src.Id == _myEmail.Id));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(src => src.Id == _myWebsite.Id));
                Assert.IsTrue(contact.ImResult.Values.Any(src => src.Id == _myIm.Id));
            }
        }

        [TestMethod]
        public void ChangeContact()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {Id = _myContactId});

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {Id = _myContactId, FirstName = "Eddy", BirthDate = new DateTime(1980, 8, 22)});

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(contact.Id, result.Id);
                Assert.AreEqual(contact.Id, result.Values.First().Id);
                Assert.AreEqual("Eddy", result.Values.First().FirstName);
                Assert.AreEqual(new DateTime(1980, 8, 22), result.Values.First().BirthDate);
            }
        }

        [TestMethod]
        public void ChainedWebsiteCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var myWebsite = new Website {Url = "http://smurf.com"};

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions {Match = "external_identifier"},
                        WebsiteSaveRequest = new List<Website> { myWebsite }
                    });
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {Id = result.Id.Value, WebsiteGetRequest = new BaseRequest()});

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite.Url, contact.WebsiteResult.Values.First().Url);

                // Delete contact

                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(result.Id.Value), 1);

            }
        }

        /// <summary>
        /// Unit test for avoiding upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15815
        /// </summary>
        /// <remarks>
        /// See also: https://github.com/johanv/civicrm.net/issues/39
        /// </remarks>
        [TestMethod]
        public void ChainedPhoneAndWebsiteCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain phone and website.
                var myPhone = new Phone { PhoneNumber = "03-100 20 00"};
                var myWebsite = new Website { Url = "http://smurf.com" };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        PhoneSaveRequest = new List<Phone> {myPhone},
                        WebsiteSaveRequest = new List<Website> { myWebsite }
                    });

                // TODO: New workaround for upstream issue CRM-15815:
                Assert.IsNotNull(result.Id);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        PhoneGetRequest =  new BaseRequest(),
                        WebsiteGetRequest = new BaseRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(result.Id.Value), 1);

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(1, contact.PhoneResult.Count);
                Assert.AreEqual(myPhone.PhoneNumber, contact.PhoneResult.Values.First().PhoneNumber);
                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite.Url, contact.WebsiteResult.Values.First().Url);
            }
        }

        /// <summary>
        /// Unit test for upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15815
        /// </summary>
        [TestMethod]
        public void ChainedPhoneAnd2WebsitesCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain phone and website.
                var myPhone = new Phone { PhoneNumber = "03-100 20 00" };
                var myWebsite1 = new Website { Url = "http://smurf.com" };
                var myWebsite2 = new Website { Url = "http://smurf.org" };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions {Match = "external_identifier"},
                        WebsiteSaveRequest = new List<BaseRequest> {myWebsite1, myWebsite2},
                        PhoneSaveRequest = new List<BaseRequest> {myPhone},
                    });

                // This crashes because of upstream issue CRM-15815:
                Assert.IsNotNull(result.Id);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        PhoneGetRequest = new BaseRequest(),
                        WebsiteGetRequest = new BaseRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(result.Id.Value), 1);

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(1, contact.PhoneResult.Count);
                Assert.AreEqual(myPhone.PhoneNumber, contact.PhoneResult.Values.First().PhoneNumber);
                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite1.Url, contact.WebsiteResult.Values.First().Url);
                Assert.AreEqual(myWebsite2.Url, contact.WebsiteResult.Values.First().Url);
            }
        }

        [TestMethod]
        public void ChainedWebsiteCreateTwo()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var my1StWebsite = new Website { Url = "http://smurf.com" };
                var my2NdWebsite = new Website {Url = "http://salsaparilla.org"};

                var newContact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    LastName = "Smurf",
                    FirstName = "Smul",
                    ExternalIdentifier = "Test_External_Smurf",
                    ApiOptions = new ApiOptions {Match = "external_identifier"},
                    WebsiteSaveRequest = new List<BaseRequest> {my1StWebsite, my2NdWebsite},
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, newContact);
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        WebsiteGetRequest = new BaseRequest()
                    });
                Assert.IsNotNull(contact.Id);

                // Clean up first (delete contact), then do other assertions.
                // (So the contact gets deleted even if the assertions fail.)
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(contact.Id.Value), 1);

                Assert.IsNotNull(contact.Id);
                Assert.AreEqual(newContact.ExternalIdentifier, contact.ExternalIdentifier);
                Assert.AreEqual(2, contact.WebsiteResult.Count);
                Assert.IsTrue(contact.WebsiteResult.Values.Any(ws => ws.Url == my1StWebsite.Url));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(ws => ws.Url == my2NdWebsite.Url));
            }
        }

        [TestMethod]
        public void CreateContact()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    FirstName = "Lucky",
                    LastName = "Luke",
                    BirthDate = new DateTime(1946, 3, 3),
                    Gender = Gender.Male,
                    ExternalIdentifier = "test_ext_id_yep",
                    ApiOptions = new ApiOptions {Match = "external_identifier"}
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.IsNotNull(result.Id);
                Assert.AreEqual(result.Id, result.Values.First().Id);
                Assert.AreEqual(contact.FirstName, result.Values.First().FirstName);
                Assert.AreEqual(contact.LastName, result.Values.First().LastName);
                Assert.AreEqual(contact.BirthDate, result.Values.First().BirthDate);
                Assert.AreEqual(contact.Gender, result.Values.First().Gender);

                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(result.Id.Value), 1);
            }
        }

        [TestMethod]
        public void ApiOptions()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    ExternalIdentifier = _myContact.ExternalIdentifier,
                    FirstName = "Wesley",
                    LastName = "Decabooter",
                    // use external ID to find the contact, instead of contact id.
                    ApiOptions = new ApiOptions { Match = "external_identifier" }
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(_myContact.Id, result.Id);
                Assert.AreEqual(_myContact.Id, result.Values.First().Id);
                Assert.AreEqual(contact.FirstName, result.Values.First().FirstName);
                Assert.AreEqual(contact.LastName, result.Values.First().LastName);
            }
        }

        [TestMethod]
        public void EnumProperties()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {ExternalIdentifier = _myContact.ExternalIdentifier});

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = contact.Id,
                        Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male,
                        PreferredMailFormat =
                            contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML
                    });

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(_myContact.Id, result.Id);
                Assert.AreEqual(_myContact.Id, result.Values.First().Id);
                Assert.AreNotEqual(contact.Gender, result.Values.First().Gender);
                Assert.AreNotEqual(contact.PreferredMailFormat, result.Values.First().PreferredMailFormat);
            }
        }

        [TestMethod]
        public void ChainedSearchRequest()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var my1StWebsite = new Website { Url = "http://smurf.com", WebsiteType = WebsiteType.Main};
                var my2NdWebsite = new Website { Url = "http://salsaparilla.org", WebsiteType = WebsiteType.Work};

                var newContact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    LastName = "Smurf",
                    FirstName = "Smul",
                    ExternalIdentifier = "Test_External_Smurf",
                    ApiOptions = new ApiOptions { Match = "external_identifier" },
                    WebsiteSaveRequest = new List<BaseRequest> { my1StWebsite, my2NdWebsite }
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, newContact);
                Debug.Assert(result.Id.HasValue);

                // Get contact with main websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        WebsiteGetRequest = new Website {WebsiteType = WebsiteType.Main}
                    });
                Assert.IsNotNull(contact.Id);

                // Clean up first (delete contact), then do other assertions.
                // (So the contact gets deleted even if the assertions fail.)
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(contact.Id.Value), 1);

                Assert.IsTrue(contact.WebsiteResult.Values.All(w => w.WebsiteType == WebsiteType.Main));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(w => w.Url == my1StWebsite.Url));
            }
        }

    }

    /// <summary>
    /// Some custom request for testing.
    /// </summary>
    internal class CustomWebsiteRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "website_type_id")]
        public WebsiteType WebsiteType { get; set; }
    }
}
