﻿/*
   Copyright 2014-2016 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using NUnit.Framework;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestFixture]
    public class ContactTest
    {
        private int _myContactId;
        private int _myDeceasedContactId;
        private int _myAddressId;
        private int _myPhoneId;
        private int _myEmailId;
        private int _myImId;
        private int _myWebsiteId;

        private const string MyExternalId = "Unit_Test_External_ID";

        [SetUp]
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
                        ExternalIdentifier = MyExternalId,
                        AddressSaveRequest = new[]
                        {
                            new AddressRequest
                            {
                                StreetAddress = "Kipdorp 30",
                                PostalCode = "2000",
                                City = "Antwerpen",
                                CountryId = 1020, // Belgium
                                LocationTypeId = 1,
                            }
                        },
                        PhoneSaveRequest = new[] {new PhoneRequest {PhoneNumber = "02-345 67 89", PhoneType = PhoneType.Phone}},
                        EmailSaveRequest = new[] {new EmailRequest {EmailAddress = "joe@schmoe.com"}},
                        WebsiteSaveRequest = new[]
                        {
                            new WebsiteRequest
                            {
                                Url = "https://twitter.com/jschmoe",
                                WebsiteType = WebsiteType.Twitter
                            }
                        },
                        ImSaveRequest = new[] {new ImRequest {Name = "joe.schmoe@facebook.com", Provider = Provider.Jabber}},
                        // If the contact with given external identifier already exists,
                        // reuse it.
                        ApiOptions = new ApiOptions {Match = "external_identifier"}
                    });

                _myContactId = result.Values.First().Id;

                // Fetch contact again, because chaining, sequential and reload option don't play well
                // together. See https://issues.civicrm.org/jira/browse/CRM-15904.

                var request = new ContactRequest
                {
                    Id = _myContactId,
                    WebsiteGetRequest = new WebsiteRequest(),
                    AddressGetRequest = new AddressRequest(),
                    PhoneGetRequest = new PhoneRequest(),
                    EmailGetRequest = new EmailRequest(),
                    ImGetRequest = new ImRequest()
                };

                var result2 = client.ContactGet(TestHelper.ApiKey, TestHelper.SiteKey, request);

                int? websiteId = result2.Values.First().WebsiteResult.Values.First().Id;
                Debug.Assert(websiteId.HasValue);
                _myWebsiteId = websiteId.Value;

                int? addressId = result2.Values.First().AddressResult.Values.First().Id;
                Debug.Assert(addressId.HasValue);
                _myAddressId = addressId.Value;

                int? phoneId = result2.Values.First().PhoneResult.Values.First().Id;
                Debug.Assert(phoneId.HasValue);
                _myPhoneId = phoneId.Value;

                int? emailId = result2.Values.First().EmailResult.Values.First().Id;
                Debug.Assert(emailId.HasValue);
                _myEmailId = emailId.Value;

                int? imId = result2.Values.First().ImResult.Values.First().Id;
                Debug.Assert(imId.HasValue);
                _myImId = imId.Value;

                var deceisedContactRequest = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    FirstName = "Jolly",
                    LastName = "Jumper",
                    BirthDate = new DateTime(1946, 3, 3),
                    Gender = Gender.Male,
                    ExternalIdentifier = "test_ext_id_deceised",
                    IsDeceased = true,
                    ApiOptions = new ApiOptions { Match = "external_identifier" }
                };

                var deceisedResult = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, deceisedContactRequest);
                Debug.Assert(deceisedResult.IsError == 0);
                Debug.Assert(deceisedResult.Id.HasValue);
                _myDeceasedContactId = deceisedResult.Id.Value;
            }
        }

        [TearDown]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myContactId),
                    1);

                Debug.Assert(result.IsError == 0,
                    "Could not delete contact. Check for the delete_contact permission of your API user.");
            }
        }

        [Test]
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
                    AddressGetRequest =  new AddressRequest()
                });
                Assert.IsTrue(contact.AddressResult.Values.Any(adr => adr.Id == _myAddressId));
            }
        }

        [Test]
        public void CreateWithChainedRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var relationshipSaveRequest = new RelationshipRequest
                {
                    RelationshipTypeId = 5,
                    ContactIdB = 1,
                    ContactIdAValueExpression = "$value.id"
                };

                var contact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    FirstName = "Lucky",
                    LastName = "Luke",
                    BirthDate = new DateTime(1946, 3, 3),
                    Gender = Gender.Male,
                    ExternalIdentifier = "test_ext_id_yep",
                    RelationshipSaveRequest = new[] {relationshipSaveRequest},
                    ApiOptions = new ApiOptions {Match = "external_identifier"}
                };

                var saveResult = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);
                var getResult = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = saveResult.Id,
                        RelationshipGetRequest = new RelationshipRequest {ContactIdAValueExpression = "$value.id"}
                    });

                // Delete first (cleanup), check afterward.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(saveResult.Id.Value), 1);

                Assert.AreEqual(0, saveResult.IsError);
                Assert.AreEqual(1, getResult.RelationshipResult.Count);

                var relationShip = getResult.RelationshipResult.Values.First();

                Assert.AreEqual(relationshipSaveRequest.RelationshipTypeId, relationShip.RelationshipTypeId);
                Assert.AreEqual(relationshipSaveRequest.ContactIdB, relationShip.ContactIdB);
            }
        }

        [Test]
        public void ChainedCommunicationGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ExternalIdentifier = MyExternalId,
                        PhoneGetRequest = new PhoneRequest(),
                        EmailGetRequest = new EmailRequest(),
                        WebsiteGetRequest = new WebsiteRequest(),
                        ImGetRequest = new ImRequest()
                    });
                Assert.IsTrue(contact.PhoneResult.Values.Any(src => src.Id == _myPhoneId));
                Assert.IsTrue(contact.EmailResult.Values.Any(src => src.Id == _myEmailId));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(src => src.Id == _myWebsiteId));
                Assert.IsTrue(contact.ImResult.Values.Any(src => src.Id == _myImId));
            }
        }

        [Test]
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

        [Test]
        public void ChainedWebsiteCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var myWebsite = new WebsiteRequest {Url = "http://smurf.com"};

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions {Match = "external_identifier"},
                        WebsiteSaveRequest = new List<WebsiteRequest> { myWebsite }
                    });
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest { Id = result.Id.Value, WebsiteGetRequest = new WebsiteRequest() });

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite.Url, contact.WebsiteResult.Values.First().Url);

                // Delete contact

                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);

            }
        }

        /// <summary>
        /// Unit test for avoiding upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15815
        /// </summary>
        /// <remarks>
        /// See also: https://github.com/johanv/civicrm.net/issues/39
        /// </remarks>
        [Test]
        public void ChainedPhoneAndWebsiteCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain phone and website.
                var myPhone = new PhoneRequest { PhoneNumber = "03-100 20 00"};
                var myWebsite = new WebsiteRequest { Url = "http://smurf.com" };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        PhoneSaveRequest = new List<PhoneRequest> {myPhone},
                        WebsiteSaveRequest = new List<WebsiteRequest> { myWebsite }
                    });

                // TODO: New workaround for upstream issue CRM-15815:
                Assert.IsNotNull(result.Id);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        PhoneGetRequest =  new PhoneRequest(),
                        WebsiteGetRequest = new WebsiteRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(1, contact.PhoneResult.Count);
                Assert.AreEqual(myPhone.PhoneNumber, contact.PhoneResult.Values.First().PhoneNumber);
                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite.Url, contact.WebsiteResult.Values.First().Url);
            }
        }

        /// <summary>
        /// Unit test for avoiding upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15815
        /// </summary>
        /// <remarks>
        /// See also: https://github.com/johanv/civicrm.net/issues/39
        /// </remarks>
        [Test]
        public void EmptyChainedPhoneAndWebsiteCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact
                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        // empty requests, try to hit CRM-15815.
                        PhoneSaveRequest = new List<PhoneRequest>(),
                        WebsiteSaveRequest = new List<WebsiteRequest>()
                    });

                // TODO: New workaround for upstream issue CRM-15815:
                Assert.IsNotNull(result.Id);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        PhoneGetRequest = new PhoneRequest(),
                        WebsiteGetRequest = new WebsiteRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(0, contact.PhoneResult.Count);
                Assert.AreEqual(0, contact.WebsiteResult.Count);
            }
        }

        /// <summary>
        /// Unit test for upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15815
        /// </summary>
        [Test]
        public void ChainedPhoneAnd2WebsitesCreate()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain phone and website.
                var myPhone1 = new PhoneRequest { PhoneNumber = "03-100 20 00" }; 
                var myPhone2 = new PhoneRequest { PhoneNumber = "03-100 20 01" };
                var myWebsite1 = new WebsiteRequest { Url = "http://smurf.com" };
                var myWebsite2 = new WebsiteRequest { Url = "http://smurf.org" };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions {Match = "external_identifier"},
                        WebsiteSaveRequest = new List<WebsiteRequest> {myWebsite1, myWebsite2},
                        PhoneSaveRequest = new List<PhoneRequest> {myPhone1, myPhone2},
                    });

                // This crashes because of upstream issue CRM-15815:
                Assert.IsNotNull(result.Id);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        PhoneGetRequest = new PhoneRequest(),
                        WebsiteGetRequest = new WebsiteRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);

                Assert.AreEqual(result.Id, contact.Id);
                Assert.AreEqual(2, contact.PhoneResult.Count);
                Assert.AreEqual(myPhone1.PhoneNumber, contact.PhoneResult.Values.First().PhoneNumber);
                Assert.AreEqual(myPhone2.PhoneNumber, contact.PhoneResult.Values.Last().PhoneNumber);
                Assert.AreEqual(2, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite1.Url, contact.WebsiteResult.Values.First().Url);
                Assert.AreEqual(myWebsite2.Url, contact.WebsiteResult.Values.Last().Url);
            }
        }

        /// <summary>
        /// Test for ContactSaveWorkaroundCrm15815
        /// </summary>
        [Test]
        public void WorkaroundCrm15815()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain phone and website.
                var myPhone = new PhoneRequest { PhoneNumber = "03-100 20 00" };
                var myWebsite1 = new WebsiteRequest { Url = "http://smurf.com" };
                var myWebsite2 = new WebsiteRequest { Url = "http://smurf.org" };
                const string myExternalId = "Test_External_Smurf";

                client.ContactSaveWorkaroundCrm15815(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = myExternalId,
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        WebsiteSaveRequest = new List<WebsiteRequest> { myWebsite1, myWebsite2 },
                        PhoneSaveRequest = new List<PhoneRequest> { myPhone },
                    });

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ExternalIdentifier = myExternalId,
                        PhoneGetRequest = new PhoneRequest(),
                        WebsiteGetRequest = new WebsiteRequest(),
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(contact.Id), 1);

                Assert.AreEqual(1, contact.PhoneResult.Count);
                Assert.AreEqual(myPhone.PhoneNumber, contact.PhoneResult.Values.First().PhoneNumber);
                Assert.AreEqual(2, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite1.Url, contact.WebsiteResult.Values.First().Url);
                Assert.AreEqual(myWebsite2.Url, contact.WebsiteResult.Values.Last().Url);
            }
        }

        [Test]
        public void ChainedWebsiteCreateTwo()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var my1StWebsite = new WebsiteRequest { Url = "http://smurf.com" };
                var my2NdWebsite = new WebsiteRequest {Url = "http://salsaparilla.org"};

                var newContact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    LastName = "Smurf",
                    FirstName = "Smul",
                    ExternalIdentifier = "Test_External_Smurf",
                    ApiOptions = new ApiOptions {Match = "external_identifier"},
                    WebsiteSaveRequest = new List<WebsiteRequest> {my1StWebsite, my2NdWebsite},
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, newContact);
                Debug.Assert(result.Id.HasValue);

                // Get contact with websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        WebsiteGetRequest = new WebsiteRequest()
                    });
                Assert.IsNotNull(contact.Id);

                // Clean up first (delete contact), then do other assertions.
                // (So the contact gets deleted even if the assertions fail.)
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(contact.Id), 1);

                Assert.IsNotNull(contact.Id);
                Assert.AreEqual(newContact.ExternalIdentifier, contact.ExternalIdentifier);
                Assert.AreEqual(2, contact.WebsiteResult.Count);
                Assert.IsTrue(contact.WebsiteResult.Values.Any(ws => ws.Url == my1StWebsite.Url));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(ws => ws.Url == my2NdWebsite.Url));
            }
        }

        [Test]
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

                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);
            }
        }

        /// <summary>
        /// Contact.Get should return deceased contacts as well. (#82)
        /// </summary>
        [Test]
        public void DeceasedContactGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new ContactRequest
                {
                    Id = _myDeceasedContactId
                };

                var result = client.ContactGet(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(0, result.IsError);
                Assert.IsNotNull(result.Id);
                Assert.AreEqual(result.Id, result.Values.First().Id);
                Assert.AreEqual(_myDeceasedContactId, result.Id);
            }
        }

        [Test]
        public void CreateWithContactSubType()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = new ContactRequest
                {
                    ContactType = ContactType.Organization,
                    // this should be an existing subtype:
                    // I am not sure why the subtype should be an array.
                    ContactSubType = "Ploeg",
                    OrganizationName = "Organization X",
                    ExternalIdentifier = "test_ext_id_subtype",
                    ApiOptions = new ApiOptions { Match = "external_identifier" }
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.IsNotNull(result.Id);
                Assert.AreEqual(result.Id, result.Values.First().Id);
                Assert.AreEqual(contact.OrganizationName, result.Values.First().OrganizationName);
                Assert.AreEqual(contact.ContactSubType, result.Values.First().ContactSubType.First());

                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(result.Id.Value), 1);
            }
        }

        [Test]
        public void SortAndLimit()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contactRequest = new ContactRequest
                {
                    // Because of upstream issue CRM-15905, you cannot search contacts on 'id'.
                    // A workaround is writing 'contact_a.id'.
                    // https://issues.civicrm.org/jira/browse/CRM-15905
                    ApiOptions = new ApiOptions {Sort = "contact_a.id DESC", Limit = 2}
                };

                var result = client.ContactGet(TestHelper.ApiKey, TestHelper.SiteKey, contactRequest);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Values.First().Id > result.Values.Last().Id);
            }            
        }

        [Test]
        public void ApiOptions()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    ExternalIdentifier = MyExternalId,
                    FirstName = "Wesley",
                    LastName = "Decabooter",
                    // use external ID to find the contact, instead of contact id.
                    ApiOptions = new ApiOptions { Match = "external_identifier" }
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, contact);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(_myContactId, result.Id);
                Assert.AreEqual(_myContactId, result.Values.First().Id);
                Assert.AreEqual(contact.FirstName, result.Values.First().FirstName);
                Assert.AreEqual(contact.LastName, result.Values.First().LastName);
            }
        }

        [Test]
        public void EnumProperties()
        {
            using (var client = TestHelper.ClientGet())
            {
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {ExternalIdentifier = MyExternalId});

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = contact.Id,
                        Gender = contact.Gender == Gender.Male ? Gender.Female : Gender.Male,
                        PreferredMailFormat =
                            contact.PreferredMailFormat == MailFormat.HTML ? MailFormat.Text : MailFormat.HTML
                    });

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(_myContactId, result.Id);
                Assert.AreEqual(_myContactId, result.Values.First().Id);
                Assert.AreNotEqual(contact.Gender, result.Values.First().Gender);
                Assert.AreNotEqual(contact.PreferredMailFormat, result.Values.First().PreferredMailFormat);
            }
        }

        [Test]
        public void ChainedSearchRequest()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact, chain website.
                var my1StWebsite = new WebsiteRequest { Url = "http://smurf.com", WebsiteType = WebsiteType.Main};
                var my2NdWebsite = new WebsiteRequest { Url = "http://salsaparilla.org", WebsiteType = WebsiteType.Work};

                var newContact = new ContactRequest
                {
                    ContactType = ContactType.Individual,
                    LastName = "Smurf",
                    FirstName = "Smul",
                    ExternalIdentifier = "Test_External_Smurf",
                    ApiOptions = new ApiOptions { Match = "external_identifier" },
                    WebsiteSaveRequest = new List<WebsiteRequest> { my1StWebsite, my2NdWebsite }
                };

                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey, newContact);
                Debug.Assert(result.Id.HasValue);

                // Get contact with main websites
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = result.Id.Value,
                        WebsiteGetRequest = new WebsiteRequest {WebsiteType = WebsiteType.Main}
                    });
                Assert.IsNotNull(contact.Id);

                // Clean up first (delete contact), then do other assertions.
                // (So the contact gets deleted even if the assertions fail.)
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(contact.Id), 1);

                Assert.IsTrue(contact.WebsiteResult.Values.All(w => w.WebsiteType == WebsiteType.Main));
                Assert.IsTrue(contact.WebsiteResult.Values.Any(w => w.Url == my1StWebsite.Url));
            }
        }

        [Test]
        public void ChainedOptionsWebsite()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact with two websites.
                var myWebsite1 = new WebsiteRequest { Url = "http://smurf1.com" };
                var myWebsite2 = new WebsiteRequest { Url = "http://smurf2.com" };

                var saveResult = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        WebsiteSaveRequest = new List<WebsiteRequest> { myWebsite1, myWebsite2 },
                    });
                Assert.IsNotNull(saveResult.Id);

                // Get contact with websites, order them backwards, and retrieve only one.
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = saveResult.Id.Value,
                        WebsiteGetRequest = new WebsiteRequest
                        {
                            ApiOptions = new ApiOptions {Sort = "url DESC", Limit = 1}
                        }
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(saveResult.Id.Value), 1);

                Assert.AreEqual(1, contact.WebsiteResult.Count);
                Assert.AreEqual(myWebsite2.Url, contact.WebsiteResult.Values.First().Url);
            }
        }

        /// <summary>
        /// Unit test for upstream issue:
        /// https://issues.civicrm.org/jira/browse/CRM-15983
        /// </summary>
        [Test]
        public void ChainedOptionsRelationships()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Create a contact with two relationships.

                var relationshipRequest1 = new RelationshipRequest
                {
                    RelationshipTypeId = 5, // Works for
                    ContactIdAValueExpression = "$value.id",
                    ContactIdB = 1,         // Default organization
                    StartDate = DateTime.Now.Date.AddYears(-2),
                    EndDate = DateTime.Now.Date.AddYears(-1),
                    IsActive = false
                };
                var relationshipRequest2 = new RelationshipRequest
                {
                    RelationshipTypeId = 5, // Works for
                    ContactIdAValueExpression = "$value.id",
                    ContactIdB = 1,         // Default organization
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddYears(1),
                    IsActive = true
                };

                var saveResult = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Individual,
                        LastName = "Smurf",
                        FirstName = "Smul",
                        ExternalIdentifier = "Test_External_Smurf",
                        ApiOptions = new ApiOptions { Match = "external_identifier" },
                        RelationshipSaveRequest = new List<RelationshipRequest> { relationshipRequest1, relationshipRequest2 },
                    });
                Assert.IsNotNull(saveResult.Id);

                // Get contact with relationships, order them backwards, and retrieve only one.
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        Id = saveResult.Id.Value,
                        RelationshipGetRequest = new RelationshipRequest
                        {
                            ContactIdAValueExpression = "$value.id",
                            ApiOptions = new ApiOptions {Sort = "end_date DESC", Limit = 1}
                        }
                    });

                // Delete contact before doing assertions.
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(saveResult.Id.Value), 1);

                Assert.AreEqual(1, contact.RelationshipResult.Count);
                Assert.AreEqual(relationshipRequest2.StartDate, contact.RelationshipResult.Values.First().StartDate);
            }
        }

    }
}
