/*
   Copyright 2014, 2015 Chirojeugd-Vlaanderen vzw

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
        private const string MyAddressName = "Nationaal Secretariaat";

        private int _myContactId;
        private int _myAddressId;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                // ContactType seems to be mandatory for a create action of the CiviCRM
                // contact api.
                var result = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest {ContactType = ContactType.Individual, FirstName = "Joe", LastName = "Schmoe"});
                _myContactId = result.Values.First().Id;
                // TODO: chain this address creation.
                var addressRequest = new AddressRequest
                {
                    ContactId = _myContactId,
                    StreetAddress = "Kipdorp 30",
                    PostalCode = "2000",
                    City = "Antwerpen",
                    CountryId = 1020,   // Belgium
                    LocationTypeId = 1,
                    Name = MyAddressName
                };
                // If this fails, please turn off map and geocode services.
                // (Adminis, System Settings, Maps)
                var addressResult = client.AddressSave(TestHelper.ApiKey, TestHelper.SiteKey, addressRequest);
                var address = addressResult.Values.First();
                _myAddressId = address.Id;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myContactId),
                    1);
            }
        }

        [TestMethod]
        public void AddAddress()
        {
            using (var client = TestHelper.ClientGet())
            {
                var newAddress = new AddressRequest
                {
                    ContactId = _myContactId,
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

        [TestMethod]
        public void GetAddress()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.AddressGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(_myAddressId));
                Assert.AreEqual(_myAddressId, result.Id);
                Assert.AreEqual(MyAddressName, result.Name);
            }
        }

        [TestMethod]
        public void DeleteAddress()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new IdRequest(_myAddressId);

                var result = client.AddressDelete(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(0, result.IsError);
            }
        }
    }
}
