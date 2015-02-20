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
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class MembershipTest
    {
        // We assume that the membershiptype with ID 1 exists in the CiviCRM instance.
        private int _someMembershipTypeId = 1;
        private int _myContactId;

        [TestInitialize]
        public void InitializeTests()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result1 = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        FirstName = "Joe",
                        LastName = "Schmoe",
                        BirthDate = new DateTime(1980, 2, 9),
                        ContactType = ContactType.Individual
                    });
                _myContactId = result1.Values.First().Id;                
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
        public void CreateMembership()
        {
            using (var client = TestHelper.ClientGet())
            {
                var membershipRequest = new MembershipRequest
                {
                    MembershipTypeId = _someMembershipTypeId,
                    ContactId = _myContactId,
                    JoinDate = DateTime.Now.Date,
                    StartDate = DateTime.Now.Date,
                    Status = MembershipStatus.Pending
                };

                var result = client.MembershipSave(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);
                var newMembership = result.Values.First();

                Assert.AreEqual(membershipRequest.MembershipTypeId, newMembership.MembershipTypeId);
                Assert.AreEqual(membershipRequest.ContactId, newMembership.ContactId);
                Assert.AreEqual(membershipRequest.JoinDate, newMembership.JoinDate);
                Assert.AreEqual(membershipRequest.StartDate, newMembership.StartDate);
                Assert.AreEqual(membershipRequest.Status, newMembership.Status);
            }
        }
    }
}
