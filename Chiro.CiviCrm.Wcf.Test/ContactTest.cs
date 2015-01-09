/*
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
using System.Diagnostics;
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                    new Contact
                    {
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
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest
                {
                    Id = _myContactId,
                    // We don't need all fields of the contact, we are only interested in the
                    // addresses.

                    // ReturnFields are still in civicrm notation, meaning lowercase and
                    // underscores (see issue #19)
                    ReturnFields = "id",
                    ChainedEntities = new[] { CiviEntity.Address }
                });
                Assert.IsTrue(contact.ChainedAddresses.Values.Any(adr => adr.Id == _myAddress.Id));
            }
        }

        [TestMethod]
        public void ChainedCommunicationGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ExternalIdentifierRequest
                    {
                        ExternalIdentifier = _myContact.ExternalIdentifier,
                        ChainedEntities = new[] {CiviEntity.Phone, CiviEntity.Email, CiviEntity.Website, CiviEntity.Im}
                    });
                Assert.IsTrue(contact.ChainedPhones.Values.Any(src => src.Id == _myPhone.Id));
                Assert.IsTrue(contact.ChainedEmails.Values.Any(src => src.Id == _myEmail.Id));
                Assert.IsTrue(contact.ChainedWebsites.Values.Any(src => src.Id == _myWebsite.Id));
                Assert.IsTrue(contact.ChainedIms.Values.Any(src => src.Id == _myIm.Id));
            }
        }

        [TestMethod]
        public void ChangeContact()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myContactId));

                contact.FirstName = "Eddy";
                contact.BirthDate = new DateTime(1980, 8, 22);

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(contact.Id, result.Id);
                Assert.AreEqual(contact.Id, result.Values.First().Id);
                Assert.AreEqual(contact.FirstName, result.Values.First().FirstName);
                Assert.AreEqual(contact.BirthDate, result.Values.First().BirthDate);
            }
        }

        [TestMethod]
        public void CreateContact()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = new Contact
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
                var contact = new Contact
                {
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
                    new ExternalIdentifierRequest(_myContact.ExternalIdentifier));

                contact.Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male;
                contact.PreferredMailFormat = contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML;

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(_myContact.Id, result.Id);
                Assert.AreEqual(_myContact.Id, result.Values.First().Id);
                Assert.AreEqual(contact.Gender, result.Values.First().Gender);
                Assert.AreEqual(contact.PreferredMailFormat, result.Values.First().PreferredMailFormat);
            }
        }
    }
}
