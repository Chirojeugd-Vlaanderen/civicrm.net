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
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class EventTest
    {
        // Make sure that you have an event type with given ID:
        private const int MyEventTypeId = 1;

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
                    StartDate = new DateTime(2015, 07, 01),
                    EndDate = new DateTime(2015, 07, 10),
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
    }
}
