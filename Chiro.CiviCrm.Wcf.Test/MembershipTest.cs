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
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using NUnit.Framework;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestFixture]
    public class MembershipTest
    {
        // We assume that the membershiptype with ID 1 exists in the CiviCRM instance.
        private const int SomeMembershipTypeId = 1;

        private int _myContactId1;
        private int _myContactId2;

        [SetUp]
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
                _myContactId1 = result1.Values.First().Id;

                var result2 = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        FirstName = "Jesse",
                        LastName = "Schmoe",
                        BirthDate = new DateTime(1980, 2, 9),
                        ContactType = ContactType.Individual,
                        MembershipSaveRequest =
                            new[]
                            {
                                new MembershipRequest
                                {
                                    MembershipTypeId = 1,
                                    Status = MembershipStatus.New,
                                    StartDate = new DateTime(2016, 09, 21)
                                }
                            }
                    });
                _myContactId2 = result2.Values.First().Id;                

            }
        }

        [TearDown]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myContactId1),
                    1);
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myContactId2),
                    1);
            }
        }

        [Test]
        public void CreateMembership()
        {
            // If this test fails, check whether your API user has the permissions
            // access civimember and edit memberships.
            using (var client = TestHelper.ClientGet())
            {
                var membershipRequest = new MembershipRequest
                {
                    MembershipTypeId = SomeMembershipTypeId,
                    ContactId = _myContactId1,
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

        [Test]
        public void GetMembership()
        {
            var membershipRequest = new MembershipRequest
            {
                ContactId = _myContactId2
            };
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);

                Assert.AreEqual(1, result.Count);
            }
        }

        [Test]
        public void GetMembershipStatusFilter()
        {
            var membershipRequest = new MembershipRequest
            {
                ContactId = _myContactId2,
                StatusFilter = new Filter<MembershipStatus>(WhereOperator.In, MembershipStatus.New, MembershipStatus.Current)
            };
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);

                Assert.AreEqual(1, result.Count);
            }
        }

        /// <summary>
        /// Test for filtering on relationship date.
        /// </summary>
        [Test]
        public void RelationshipDateFilter()
        {
            var membershipRequest = new MembershipRequest
            {
                ContactId = _myContactId2,
                StartDateFilter = new Filter<DateTime?>(WhereOperator.Gte, new DateTime(2016, 09, 21))
            };
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);

                Assert.AreEqual(1, result.Count);
            }
        }

        /// <summary>
        /// Just a test for the MembershipPaymentRequest
        /// </summary>
        [Test]
        public void GetMembershipWithPayment()
        {
            var membershipRequest = new MembershipRequest
            {
                MembershipPaymentGetRequest = new MembershipPaymentRequest()
            };
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);

                Assert.IsTrue(result.Count >= 1);
                var first = result.Values.First();
                Assert.IsNotNull(first.MembershipPaymentResult);
            }
        }

        /// <summary>
        /// Test chaining ContactRequests.
        /// </summary>
        [Test]
        public void GetMembershipWithContact()
        {
            var membershipRequest = new MembershipRequest
            {
                ContactGetRequest = new ContactRequest(),
                ContactId = _myContactId2
            };
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey, membershipRequest);

                Assert.IsTrue(result.Count >= 1);
                var first = result.Values.First();
                Assert.IsNotNull(first.ContactResult);
                Assert.AreEqual(_myContactId2, first.ContactResult.Values.First().Id);
            }
        }
    }
}
