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
using System.Diagnostics;
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class EventTest
    {
        // Make sure that you have an event type with given ID:
        private const int MyEventTypeId = 1;

        private int _myEventId;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var eventRequest = new EventRequest
                {
                    Title = "Belgian Beer Event",
                    Description = "Best event ever.",
                    StartDate = new Filter<DateTime?>(new DateTime(2016, 02, 05)),
                    EndDate = new Filter<DateTime?>(new DateTime(2016, 02, 05)),
                    EventTypeId = MyEventTypeId
                };

                var saveResult = client.EventSave(TestHelper.ApiKey, TestHelper.SiteKey, eventRequest);
                Debug.Assert(saveResult != null);
                Debug.Assert(saveResult.Id.HasValue);

                _myEventId = saveResult.Id.Value;
            }
        }

        [TestMethod]
        public void GetEventDateFilter()
        {
            DateTime someDate = new DateTime(2015, 3, 14);
            using (var client = TestHelper.ClientGet())
            {
                var request = new EventRequest
                {
                    StartDate = new Filter<DateTime?> (WhereOperator.Gt, someDate)
                };

                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey, request);
                Assert.IsTrue(result.Values.All(v => v.StartDate > someDate));
            }
        }

        /// <summary>
        /// Check NOT NULL-filtering (see #89).
        /// </summary>
        [TestMethod]
        public void GetEventDateFilterNotNull()
        {
            DateTime someDate = new DateTime(2015, 3, 14);
            using (var client = TestHelper.ClientGet())
            {
                var request = new EventRequest
                {
                    StartDate = new Filter<DateTime?>(WhereOperator.IsNotNull)
                };

                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey, request);
                Assert.IsTrue(result.Values.All(v => v.StartDate != null));
            }
        }

        [TestMethod]
        public void CreateEvent()
        {
            // Make sure that your API user has permissions
            // 'access CiviEvent', 'view event info',
            // and 'edit all events'.
            using (var client = TestHelper.ClientGet())
            {
                var eventRequest = new EventRequest
                {
                    Title = "My mighty event",
                    Description = "It will be fun.",
                    StartDate = new Filter<DateTime?>(new DateTime(2015, 07, 01)),
                    EndDate = new Filter<DateTime?>(new DateTime(2015, 07, 10)),
                    EventTypeId = MyEventTypeId
                };

                var saveResult = client.EventSave(TestHelper.ApiKey, TestHelper.SiteKey, eventRequest);
                Assert.IsNotNull(saveResult.Id);

                // Delete first. Then assert.
                var deleteResult = client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(saveResult.Id.Value));

                Assert.AreEqual(0, saveResult.IsError);                
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myEventId));

                Debug.Assert(result.IsError == 0,
                    "Could not delete event. Maybe your API user needs more permissions.");
            }
        }

        /// <summary>
        /// Test getting an event with locblock and address
        /// </summary>
        /// <remarks>
        /// This test may fail because of CRM-18535. You can work around it by
        /// giving your API user the 'access deleted contacts' permission.
        /// </remarks>
        [TestMethod]
        public void EventWithAddress()
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
                Title = "My mighty event",
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
                    EventSaveRequest = new[] {myEventRequest}
                });
                Assert.IsNotNull(saveResult.Id);

                int myLocBlockId = saveResult.Id.Value;

                var eventGetRequest = new EventRequest
                {
                    LocBlockId = myLocBlockId,
                    LocBlockGetRequest = new LocBlockRequest
                    {
                        IdValueExpression = "$value.loc_block_id",
                        AddressGetRequest = new AddressRequest
                        {
                            IdValueExpression = "$value.address_id"
                        }
                    }
                };
                var getResult = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey, eventGetRequest);
                Assert.AreEqual(1, getResult.Count);

                var savedEvent = getResult.Values.First();
                Assert.AreEqual(1, savedEvent.LocBlockResult.Count);
                Assert.AreEqual(1, savedEvent.LocBlockResult.Values.First().AddressResult.Count);
                var savedAddress = savedEvent.LocBlockResult.Values.First().AddressResult.Values.First();

                // Delete first. Then assert.
                client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(savedEvent.Id));
                client.LocBlockDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(myLocBlockId));
                client.AddressDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(savedAddress.Id));

                Assert.AreEqual(myEventRequest.Title, savedEvent.Title);
                Assert.AreEqual(myAddressRequest.StreetAddress, savedAddress.StreetAddress);
            }
        }

        [TestMethod]
        public void UpdateEvent()
        {
            // Make sure that your API user has permissions
            // 'access CiviEvent', 'view event info',
            // and 'edit all events'.
            using (var client = TestHelper.ClientGet())
            {
                var eventRequest = new EventRequest
                {
                    Id = _myEventId,
                    Title = "O bierbaar Belgie"
                };

                var saveResult = client.EventSave(TestHelper.ApiKey, TestHelper.SiteKey, eventRequest);

                Assert.AreEqual(_myEventId, saveResult.Id);
                Assert.AreEqual(eventRequest.Title, saveResult.Values.First().Title);
                Assert.AreEqual(0, saveResult.IsError);
            }
        }
    }
}
