/*
   Copyright 2014,2015 Johan Vervloet

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
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class WebsiteTest
    {
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
                        ApiOptions = new ApiOptions {Match = "external_identifier"}
                    });

                _myContactId = result.Values.First().Id;
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
        public void CreateWebsite()
        {
            using (var client = TestHelper.ClientGet())
            {
                var website = new Website
                {
                    ContactId = _myContactId,
                    Url = "http://blog.johanv.org",
                    WebsiteType = WebsiteType.Main
                };

                var result = client.WebsiteSave(TestHelper.ApiKey, TestHelper.SiteKey, website);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(website.ContactId, result.Values.First().ContactId);
                Assert.AreEqual(website.Url, result.Values.First().Url);
                Assert.AreEqual(website.WebsiteType, result.Values.First().WebsiteType);
            }
        }

        [TestMethod]
        public void OffsetOption()
        {
            using (var client = TestHelper.ClientGet())
            {
                var website1 = new Website
                {
                    ContactId = _myContactId,
                    Url = "http://blog.johanv.org",
                    WebsiteType = WebsiteType.Main
                };
                var website2 = new Website
                {
                    ContactId = _myContactId,
                    Url = "http://civicrm.org",
                    WebsiteType = WebsiteType.Main
                };
                var website3 = new Website
                {
                    ContactId = _myContactId,
                    Url = "http://www.chiro.be",
                    WebsiteType = WebsiteType.Main
                };


                client.WebsiteSave(TestHelper.ApiKey, TestHelper.SiteKey, website1);
                client.WebsiteSave(TestHelper.ApiKey, TestHelper.SiteKey, website2);
                client.WebsiteSave(TestHelper.ApiKey, TestHelper.SiteKey, website3);

                var result = client.WebsiteGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new CustomWebsiteRequest {ContactId = _myContactId, ApiOptions = new ApiOptions{Sort = "url", Offset = 2}});

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(website3.Url, result.Values.First().Url);
            }
        }
    }
}
