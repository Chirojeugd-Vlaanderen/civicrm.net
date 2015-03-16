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
                    StartDate = new Filter<DateTime?> {Operator = WhereOperator.Gt, Value = someDate}
                };

                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey, request);
                Assert.IsTrue(result.Values.All(v => v.StartDate > someDate));
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
                    new IdRequest(saveResult.Id.Value));

                Assert.AreEqual(0, saveResult.IsError);                
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myEventId));

                Debug.Assert(result.IsError == 0,
                    "Could not delete event. Maybe your API user needs more permissions.");
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
