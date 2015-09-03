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
    public class RelationshipTest
    {
        private int _myContactId;
        private int _myRelationshipId;
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
                        RelationshipTypeId = 5,             // Works for
                        ContactIdA = _myContactId,
                        ContactIdB = _myCompanyId,
                        IsActive = true
                    });
                _myRelationshipId = result3.Values.First().Id;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new DeleteRequest(_myContactId),
                    1);
            }
        }
        [TestMethod]
        public void GetRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new RelationshipRequest{Id = _myRelationshipId});
                var relationship = result.Values.First();

                Assert.AreEqual(_myContactId, relationship.ContactIdA);
                Assert.AreEqual(_myCompanyId, relationship.ContactIdB);
                Assert.AreEqual(5, relationship.RelationshipTypeId);
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
                    ContactIdB = 1,         // Default organization
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
    }
}
