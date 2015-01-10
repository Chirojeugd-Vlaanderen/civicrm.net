/*
   Copyright 2014 Johan Vervloet

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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chiro.CiviCrm.Wcf.Test
{
    [TestClass]
    public class ConnectivityTest
    {
        /// <summary>
        /// Try to retrieve contact 1, which is the default organization.
        /// </summary>
        [TestMethod]
        public void Contact1GetTest()
        {
            using (var client = TestHelper.ClientGet())
            {
                // Get the contact, and chain the contact's addresses.
                var resultValue = client.ContactGet(TestHelper.ApiKey, TestHelper.SiteKey, new IdRequest(1));

                Assert.AreEqual(0, resultValue.IsError);
                Assert.AreEqual(1, resultValue.Count);
                Assert.AreEqual(1, resultValue.Id);
                Assert.AreEqual(1, resultValue.Values.First().Id);
            }
        }
    }
}
