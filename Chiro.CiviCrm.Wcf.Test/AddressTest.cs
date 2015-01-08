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

using System.Linq;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class AddressTest
    {
        private Contact _myContact;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new Contact {FirstName = "Joe", LastName = "Schmoe"});
                _myContact = result.Values.First();
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
        public void AddAddressTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                // This is not a unit test, because one test checks multiple things.
                // I guess it is better than nothing.

                var newAddress = new Address
                {
                    ContactId = _myContact.Id,
                    StreetAddress = "Hoefslagstraatje 2",
                    PostalCode = "9000",
                    City = "Gent",
                    CountryId = 1020,   // Belgium
                    LocationTypeId = 1,
                };

                var result = client.AddressSave(TestHelper.ApiKey, TestHelper.SiteKey, newAddress);
                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(1, result.Count);

                var resultAddress = result.Values.First();

                Assert.AreEqual(newAddress.ContactId, resultAddress.ContactId);
                Assert.AreEqual(newAddress.StreetAddress, resultAddress.StreetAddress);
                Assert.AreEqual(newAddress.PostalCode, resultAddress.PostalCode);
                Assert.AreEqual(1020, resultAddress.CountryId);
                Assert.AreEqual(newAddress.LocationTypeId, resultAddress.LocationTypeId);
            }
        }
    }
}
