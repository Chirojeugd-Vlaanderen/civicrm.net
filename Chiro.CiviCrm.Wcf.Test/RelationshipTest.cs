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
using System.Collections.Generic;
using System.Linq;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Filters;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class RelationshipTest
    {
        private int _myContactId;
        private int _myOtherContactId;
        private int _myRelationshipId;
        private int _myOtherRelationshipId;
        private int _myCompanyId;

        [TestInitialize]
        public void InitializeTest()
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

                var result0 = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        FirstName = "Averell",
                        LastName = "Schmoe",
                        BirthDate = new DateTime(1980, 2, 9),
                        ContactType = ContactType.Individual
                    });
                _myOtherContactId = result0.Values.First().Id;

                var result2 = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Organization,
                        OrganizationName = "Schmoe inc."
                    });
                _myCompanyId = result2.Values.First().Id;

                var result3 = client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest
                    {
                        RelationshipTypeId = 5, // Works for
                        ContactIdA = _myContactId,
                        ContactIdB = _myCompanyId,
                        IsActive = true
                    });
                _myRelationshipId = result3.Values.First().Id;

                var result4 = client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest
                    {
                        RelationshipTypeId = 5, // Works for
                        ContactIdA = _myOtherContactId,
                        ContactIdB = _myCompanyId,
                        IsActive = false,
                        EndDate = new DateTime(2016, 8, 8)
                    });
                _myOtherRelationshipId = result4.Values.First().Id;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                client.RelationshipDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(_myRelationshipId));
                client.RelationshipDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myOtherRelationshipId));
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myContactId),
                    1);
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myOtherContactId),
                    1);
                client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myCompanyId),
                    1);
            }
        }

        [TestMethod]
        public void GetRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest {Id = _myRelationshipId});
                var relationship = result.Values.First();

                Assert.AreEqual(_myContactId, relationship.ContactIdA);
                Assert.AreEqual(_myCompanyId, relationship.ContactIdB);
                Assert.AreEqual(5, relationship.RelationshipTypeId);
            }
        }

        /// <summary>
        /// Test for chaining relationship -> relationship.create (see #93)
        /// </summary>
        [TestMethod]
        public void RelationshipChainedSave()
        {
            using (var client = TestHelper.ClientGet())
            {
                DateTime today = DateTime.Today;

                // Fire all contacts working for the company.
                var request = new RelationshipRequest
                {
                    RelationshipTypeId = 5, // Works for
                    ContactIdB = _myCompanyId,
                    IsActive = true,
                    RelationshipSaveRequest = new[]
                    {
                        new RelationshipRequest
                        {
                            IdValueExpression = "$value.id",
                            IsActive = false,
                            EndDate = today
                        }
                    }
                };
                // Get relationship to see if it worked.
                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey, request);
                var relationship = client.RelationshipGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest {Id = _myRelationshipId});
                Assert.IsFalse(relationship.IsActive);
                Assert.AreEqual(today, relationship.EndDate);
            }
        }

        [TestMethod]
        public void NewRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var relationshipRequest = new RelationshipRequest
                {
                    RelationshipTypeId = 5, // Works for
                    ContactIdA = _myContactId,
                    ContactIdB = 1, // Default organization
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddYears(1),
                    IsActive = true
                };

                var result = client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey, relationshipRequest);
                var newRelationship = result.Values.First();

                Assert.AreEqual(relationshipRequest.ContactIdA, newRelationship.ContactIdA);
                Assert.AreEqual(relationshipRequest.ContactIdB, newRelationship.ContactIdB);
                Assert.AreEqual(relationshipRequest.StartDate, newRelationship.StartDate);
                Assert.AreEqual(relationshipRequest.EndDate, newRelationship.EndDate);
                Assert.AreEqual(relationshipRequest.IsActive, newRelationship.IsActive);
                Assert.AreEqual(relationshipRequest.RelationshipTypeId, newRelationship.RelationshipTypeId);
            }
        }

        /// <summary>
        /// Test for resetting a relationship's end date (see #93).
        /// </summary>
        [TestMethod]
        public void ReactivateRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                // reactivate the inactive relationship.
                var saveRequest = new RelationshipRequest
                {
                    Id = _myOtherRelationshipId,
                    IsActive = true,
                    // This will be the workaround to unset a date:
                    EndDate = DateTime.MinValue
                };
                client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey, saveRequest);

                var result = client.RelationshipGetSingle(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest {Id = _myOtherRelationshipId});

                Assert.IsNull(result.EndDate);
                Assert.IsTrue(result.IsActive);
            }
        }

        [TestMethod]
        public void RelationshipChainedContact()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new RelationshipRequest
                {
                    Id = _myRelationshipId,
                    ContactGetRequest = new ContactRequest
                    {
                        IdValueExpression = "$value.contact_id_a"
                    }
                };

                var result = client.RelationshipGetSingle(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.IsNotNull(result.ContactResult);
                Assert.AreEqual(1, result.ContactResult.Count);
                Assert.AreEqual(_myContactId, result.ContactResult.Values.First().Id);
            }
        }

        [TestMethod]
        public void RelationshipDateFilter()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new RelationshipRequest
                {
                    ContactIdB = _myCompanyId,
                    EndDateFilter = new Filter<DateTime?>(WhereOperator.Gt, new DateTime(2016,8,7))
                };

                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual(_myOtherRelationshipId, result.Id);
            }
        }

        [TestMethod]
        public void RelationshipDateFilter2()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new RelationshipRequest
                {
                    ContactIdB = _myCompanyId,
                    EndDateFilter = new Filter<DateTime?>(WhereOperator.Lt, new DateTime(2016, 8, 7))
                };

                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public void RelationshipChainedGetDelete()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new RelationshipRequest
                {
                    ContactIdB = _myCompanyId,
                    EndDateFilter = new Filter<DateTime?>(WhereOperator.Gt, new DateTime(2016, 8, 7)),
                    RelationshipDeleteRequest = new List<DeleteRequest> { new DeleteRequest { IdValueExpression = "$value.id"} }
                };

                // Find and remove relationship.
                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey, request);

                var result2 = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest {Id = _myOtherRelationshipId});

                Assert.AreEqual(0, result2.Count);
            }
        }

    }
}
