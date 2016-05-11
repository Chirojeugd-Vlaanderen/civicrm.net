/*
   Copyright 2015, 2016 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class LocBlockTest
    {
        // Make sure that you have an event type with given ID:
        private const int MyEventTypeId = 1;
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
                client.LocBlockDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(_myLocBlock.Id));
                if (_myLocBlock.AddressId != null)
                {
                    client.AddressDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                        new DeleteRequest(_myLocBlock.AddressId.Value));
                }
            }
        }

        /// <summary>
        /// Test chaining get address-locblock.
        /// </summary>
        /// <remarks>
        /// This test may fail because of CRM-18535. You can work around it by
        /// giving your API user the 'access deleted contacts' permission.
        /// </remarks>
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

        /// <summary>
        /// Test getting a lockblock with events.
        /// </summary>
        [TestMethod]
        public void GetLocBlockWithEvents()
        {
            // Make sure that your API user has permissions
            // 'access CiviEvent', 'view event info',
            // and 'edit all events'.

            var myAddressRequest = new AddressRequest
            {
                LocationTypeId = 1,
                StreetAddress = "Kipdorp 30",
                PostalCode = "2000",
                City = "Antwerpen"
            };

            var myEventRequest = new EventRequest
            {
                Title = "My mighty unit test event",
                Description = "It will be fun.",
                StartDate = new Filter<DateTime?>(new DateTime(2015, 07, 01)),
                EndDate = new Filter<DateTime?>(new DateTime(2015, 07, 10)),
                EventTypeId = MyEventTypeId,
            };

            using (var client = TestHelper.ClientGet())
            {
                // Save the event by chaining everything to the loc block.
                var saveResult = client.LocBlockSave(TestHelper.ApiKey, TestHelper.SiteKey, new LocBlockRequest
                {
                    Address = myAddressRequest,
                    EventSaveRequest = new[] { myEventRequest }
                });
                Assert.IsNotNull(saveResult.Id);

                int myLocBlockId = saveResult.Id.Value;
                int? myAddressId = saveResult.Values.First().AddressId;
                Assert.IsNotNull(myAddressId);

                var locBlockGetRequest = new LocBlockRequest
                {
                    Id = myLocBlockId,
                    EventGetRequest = new EventRequest
                    {
                        LocBlockIdValueExpression = "$value.id",
                    }
                };
                var getResult = client.LocBlockGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, locBlockGetRequest);

                client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(getResult.EventResult.Values.First().Id));
                client.LocBlockDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(myLocBlockId));
                client.AddressDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(myAddressId.Value));

                Assert.AreEqual(myLocBlockId, getResult.Id);
                Assert.AreEqual(1, getResult.EventResult.Count);
                var retrievedEvent = getResult.EventResult.Values.First();
                Assert.AreEqual(myEventRequest.Title, retrievedEvent.Title);
            }
        }
    }
}
