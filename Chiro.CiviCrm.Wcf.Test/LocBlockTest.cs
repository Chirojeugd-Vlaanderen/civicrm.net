/*
   Copyright 2015 Chirojeugd-Vlaanderen vzw

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
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class LocBlockTest
    {
        private const string MyAddressName = "Regionaal Testscretariaat";

        private LocBlock _myLocBlock;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var locBlockRequest = new LocBlockRequest
                {
                    Address = new AddressRequest
                    {
                        StreetAddress = "Kipdorp 30",
                        PostalCode = "2000",
                        City = "Antwerpen",
                        CountryId = 1020, // Belgium
                        LocationTypeId = 1,
                        Name = MyAddressName
                    }
                };
                _myLocBlock = client.LocBlockSave(TestHelper.ApiKey, TestHelper.SiteKey, locBlockRequest).Values.First();
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                client.LocBlockDelete(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(_myLocBlock.Id));
                if (_myLocBlock.AddressId != null)
                {
                    client.AddressDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                        new IdRequest(_myLocBlock.AddressId.Value));
                }
            }
        }

        [TestMethod]
        public void GetAddressWithLocBlock()
        {
            using (var client = TestHelper.ClientGet())
            {
                var addressRequest = new AddressRequest
                {
                    Id = _myLocBlock.AddressId,
                    LocBlockGetRequest = new LocBlockRequest
                    {
                        AddressIdValueExpression = "$value.id"
                    }
                };
                var result = client.AddressGet(TestHelper.ApiKey, TestHelper.SiteKey, addressRequest);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(_myLocBlock.Id, result.Values.First().LocBlockResult.Id);
            }
        }
    }
}
