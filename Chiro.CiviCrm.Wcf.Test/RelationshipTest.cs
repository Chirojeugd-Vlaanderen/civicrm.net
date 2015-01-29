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
using System.ServiceModel;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Api.DataContracts.Entities;
using Chiro.CiviCrm.Api.DataContracts.EntityRequests;
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
        private Contact _myContact;
        private Relationship _myRelationship;
        private Contact _myCompany;

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
                _myContact = result1.Values.First();
                Debug.Assert(_myContact.Id.HasValue);
                _myContactId = _myContact.Id.Value;

                var result2 = client.ContactSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactType = ContactType.Organization,
                        OrganizationName = "Schmoe inc."
                    });
                _myCompany = result2.Values.First();
                Debug.Assert(_myCompany.Id.HasValue);
                _myCompanyId = _myCompany.Id.Value;

                var result3 = client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new Relationship
                    {
                        RelationshipTypeId = 5,             // Works for
                        ContactIdA = _myContactId,
                        ContactIdB = _myCompanyId,
                        IsActive = true
                    });
                _myRelationship = result3.Values.First();
                Debug.Assert(_myRelationship.Id.HasValue);
                _myRelationshipId = _myRelationship.Id.Value;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.ContactDelete(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myContact.Id ?? 0),
                    1);
            }
        }
        [TestMethod]
        public void GetRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.RelationshipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new IdRequest(_myRelationshipId));
                var relationship = result.Values.First();

                Assert.AreEqual(_myRelationship.ContactIdA, relationship.ContactIdA);
                Assert.AreEqual(_myRelationship.ContactIdB, relationship.ContactIdB);
                Assert.AreEqual(_myRelationship.Description, relationship.Description);
                Assert.AreEqual(_myRelationship.StartDate, relationship.StartDate);
                Assert.AreEqual(_myRelationship.EndDate, relationship.EndDate);
                Assert.AreEqual(_myRelationship.IsActive, relationship.IsActive);
                Assert.AreEqual(_myRelationship.RelationshipTypeId, relationship.RelationshipTypeId);
            }
        }

        [TestMethod]
        public void NewRelationship()
        {
            using (var client = TestHelper.ClientGet())
            {
                var relationship = new Relationship
                {
                    RelationshipTypeId = 5, // Works for
                    ContactIdA = _myContactId,
                    ContactIdB = 1,         // Default organization
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date.AddYears(1),
                    IsActive = true
                };

                var result = client.RelationshipSave(TestHelper.ApiKey, TestHelper.SiteKey, relationship);
                var newRelationship = result.Values.First();

                Assert.AreEqual(relationship.ContactIdA, newRelationship.ContactIdA);
                Assert.AreEqual(relationship.ContactIdB, newRelationship.ContactIdB);
                Assert.AreEqual(relationship.StartDate, newRelationship.StartDate);
                Assert.AreEqual(relationship.EndDate, newRelationship.EndDate);
                Assert.AreEqual(relationship.IsActive, newRelationship.IsActive);
                Assert.AreEqual(relationship.RelationshipTypeId, newRelationship.RelationshipTypeId);
            }
        }
    }
}
