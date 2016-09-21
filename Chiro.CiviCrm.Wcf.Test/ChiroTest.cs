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
        private int _myMembershipId;
        private int _myAbonnementId;

        [TestInitialize]
        public void InitializeTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                var eventRequest = new EventRequest
                {
                    Title = "Kamp 2015",
                    StartDate = new Filter<DateTime?>(new DateTime(2015, 07, 01)),
                    EndDate = new Filter<DateTime?>(new DateTime(2015, 07, 11)),
                    EventTypeId = (int) EvenementType.Bivak,
                    // Default organisation
                    OrganiserendePloeg1Id = 1
                };

                var eventSaveResult = client.EventSave(TestHelper.ApiKey, TestHelper.SiteKey, eventRequest);
                Assert.IsNotNull(eventSaveResult.Id);
                _myEventId = eventSaveResult.Id.Value;

                var membershipRequest = new MembershipRequest
                {
                    ContactId = 2,
                    MembershipTypeId = 1,
                    VerzekeringLoonverlies = true
                };
                var membershipSaveResult = client.MembershipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    membershipRequest);
                Assert.IsNotNull(membershipSaveResult.Id);
                _myMembershipId = membershipSaveResult.Id.Value;

                var abonnementRequest = new MembershipRequest
                {
                    ContactId = 2,
                    MembershipTypeId = 2,
                    AbonnementType = AbonnementType.Digitaal
                };

                var abonnementSaveResult = client.MembershipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    abonnementRequest);
                Assert.IsNotNull(abonnementSaveResult.Id);
                _myAbonnementId = abonnementSaveResult.Id.Value;
            }
        }

        [TestCleanup]
        public void CleanupTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                client.EventDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(_myEventId));
                client.MembershipDelete(TestHelper.ApiKey, TestHelper.SiteKey, new DeleteRequest(_myMembershipId));
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

        /// <summary>
        /// Dit durft wel eens mislukken als de nummers van custom fields zijn veranderd.
        /// </summary>
        [TestMethod]
        public void AangemaaktDoorPloegId()
        {
            using (var client = TestHelper.ClientGet())
            {
                var membershipGetResult = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new MembershipRequest {ApiOptions = new ApiOptions {Limit = 1} });
                var ploegResult = client.ContactGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new ContactRequest
                    {
                        ContactSubType = "Ploeg",
                        ApiOptions = new ApiOptions {Sort = "id DESC", Limit = 1}
                    });

                var membershipSaveResult = client.MembershipSave(TestHelper.ApiKey, TestHelper.SiteKey,
                    new MembershipRequest {Id = membershipGetResult.Id, AangemaaktDoorPloegId = ploegResult.Id});

                Assert.AreEqual(0, membershipSaveResult.IsError);
            }
        }

        /// <summary>
        /// Controleer of mijn patch voor CRM-16036 wel goed is geapplyd.
        /// </summary>
        [TestMethod]
        public void ZoekOpCustomFieldCrm16036()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new EventRequest
                    {
                        OrganiserendePloeg1Id = 1,
                    });

                Assert.AreNotEqual(0, result.Count);
                Assert.AreEqual(1, result.Values.First().OrganiserendePloeg1Id);
            }
        }

        /// <summary>
        /// Combinatie zoeken op custom field en chaining lukt niet meer sinds iets
        /// dat ze upstream deden. Zie #4062.
        /// </summary>
        [TestMethod]
        public void ZoekOpCustomFieldMetChaining()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new EventRequest
                    {
                        OrganiserendePloeg1Id = 1,
                        LocBlockGetRequest = new LocBlockRequest
                        {
                            IdValueExpression = "$value.loc_block_id"
                        }
                    });

                Assert.AreNotEqual(0, result.Count);
                Assert.AreEqual(1, result.Values.First().OrganiserendePloeg1Id);
            }
        }

        [TestMethod]
        public void ChainedCallOrganiserendePloeg()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.EventGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new EventRequest
                    {
                        Id = _myEventId,
                        ContactGetRequest = new ContactRequest {IdValueExpression = "$value.custom_48_id"}
                    });

                Assert.AreEqual(1, result.Count);
                var myEvent = result.Values.First();
                Assert.AreEqual(1, myEvent.ContactResult.Count);
                var organiserendePloeg1 = myEvent.ContactResult.Values.First();
                Assert.AreEqual(1, organiserendePloeg1.Id); // Default organisation
            }
        }

        /// <summary>
        /// Kijkt na of het veld 'loonverlies' wordt gevonden.
        /// 
        /// Zie #3970.
        /// </summary>
        [TestMethod]
        public void CustomFieldLoonverlies()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new MembershipRequest {Id = _myMembershipId});

                Assert.AreEqual(1, result.Count);
                var myMembership = result.Values.First();
                Assert.AreEqual(true, myMembership.VerzekeringLoonverlies);
            }
        }

        /// <summary>
        /// Vooral eens nakijken of de lijst met actieve lidrelaties doorkomt.
        /// </summary>
        [TestMethod]
        public void DiagnosticsActieveLidRelaties()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new BaseRequest {ApiOptions = new ApiOptions {Limit = 24}};
                var result = client.ChiroDiagnosticsActieveLidRelaties(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(0, result.IsError);
                Assert.AreEqual(result.Count, result.Values.Count());
                Assert.IsTrue(result.Count <= 24);
            }
        }

        /// <summary>
        /// Vooral eens nakijken of die API iets doet.
        /// </summary>
        [TestMethod]
        public void DiagnosticsMembersVerzekerdLoonVerlies()
        {
            using (var client = TestHelper.ClientGet())
            {
                var request = new MembershipRequest
                {
                    MembershipTypeId = 1,
                    StatusFilter = new Filter<MembershipStatus>(WhereOperator.In, new [] {MembershipStatus.New, MembershipStatus.Current }),
                    VerzekeringLoonverlies = true,
                    ContactId = 2
                };
                var result = client.ChiroDiagnosticsMembersMetAd(TestHelper.ApiKey, TestHelper.SiteKey, request);

                Assert.AreEqual(0, result.IsError);
                Assert.AreNotEqual(0, result.Count);
            }
        }

        /// <summary>
        /// Kijkt na of het veld 'AbonnementType' op Membership werkt.
        /// 
        /// Zie #3970.
        /// </summary>
        [TestMethod]
        public void CustomFieldAbonnementType()
        {
            using (var client = TestHelper.ClientGet())
            {
                var result = client.MembershipGet(TestHelper.ApiKey, TestHelper.SiteKey,
                    new MembershipRequest { Id = _myAbonnementId });

                Assert.AreEqual(1, result.Count);
                var myMembership = result.Values.First();
                Assert.AreEqual(AbonnementType.Digitaal, myMembership.AbonnementType);
            }
        }
    }
}
