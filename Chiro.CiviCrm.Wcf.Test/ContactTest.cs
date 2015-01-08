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
            }
        }
        [TestMethod]
        public void ChainedAddressGet()
        {
            using (var client = TestHelper.ClientGet())
            {
                Debug.Assert(_myContact.Id.HasValue);
                var contact = client.ContactGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest
                {
                    Id = _myContact.Id.Value,
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
    }
}
