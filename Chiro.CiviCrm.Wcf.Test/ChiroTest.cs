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
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    /// <summary>
    /// Test Chirospecifieke zaken
    /// </summary>
    [TestClass]
    public class ChiroTest
    {
        private int _myEventId;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var eventRequest = new EventRequest
                {
                    Title = "Kamp 2015",
                    StartDate = new DateTime(2015, 07, 01),
                    EndDate = new DateTime(2015, 07, 11),
                    EventTypeId = (int) EvenementType.Bivak,
                    // Default organisation
                    OrganiserendePloeg1Id = 1
                };

                var saveResult = client.EventSave(TestHelper.ApiKey, TestHelper.SiteKey, eventRequest);
                Assert.IsNotNull(saveResult.Id);
                _myEventId = saveResult.Id.Value;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var deleteResult = client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myEventId));
            }
        }

        [TestMethod]
        public void OrganiserendePloeg()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new EventRequest {Id = _myEventId});

                Assert.AreEqual(1, result.OrganiserendePloeg1Id);
            }            
        }
    }
}
