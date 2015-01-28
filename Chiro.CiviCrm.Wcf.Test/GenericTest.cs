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
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class GenericTest
    {
        private Contact _myContact;

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
                        ApiOptions = new ApiOptions { Match = "external_identifier" }
                    });

                _myContact = result.Values.First();
                Debug.Assert(_myContact.Id.HasValue);
                _myContactId = _myContact.Id.Value;
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
        public void ChangeOnlyExternalIdentifier()
        {
            var request = new ContactRequest
            {
                Id = _myContactId,
                ExternalIdentifier = "Unit_Test_New_External_ID"
            };
            using (var client = TestHelper.ClientGet())
            {
                // Change only external ID.

                client.GenericCall(TestHelper.ApiKey, TestHelper.SiteKey, CiviEntity.Contact, ApiAction.Create, request);

                // Get contact again, to check whether all went fine.

                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {Id = _myContactId});

                Assert.AreEqual(request.ExternalIdentifier, contact.ExternalIdentifier);
                Assert.AreEqual(_myContact.FirstName, contact.FirstName);
                Assert.AreEqual(_myContact.LastName, contact.LastName);
            }
        }
    }
}
