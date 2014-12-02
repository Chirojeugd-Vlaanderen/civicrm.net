/*
   Copyright 2013, 2014 Chirojeugd-Vlaanderen vzw

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
using System.Runtime.Caching;
using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.ClientInterfaces;
using Chiro.CiviCrm.Domain;
using System.Diagnostics;

namespace Chiro.CiviCrm.Client
{
    /// <summary>
    /// Interface for the CiviCRM-client.
    /// </summary>
    /// <remarks>
    /// In an ideal world, the client interface should look more like the service interface.
    /// But if I use classes in the service interface, WCF sends the data as the body of the post
    /// request, and the CiviCRM API cannot handle this.
    /// I tried to customize the WCF Uri Formatter for the WebHttpBinding, but I failed. So
    /// for the moment I work around the problem in the client.
    /// </remarks>
    public class CiviCrmClient: ClientBase<ICiviCrmApi>, ICiviCrmClient
    {
        private readonly string _apiKey;
        private readonly string _key;

        private readonly MemoryCache _cache = MemoryCache.Default;
        private const string ContactIdCacheKey = "CICK{0}"; 

        /// <summary>
        /// Creates a new CiviCRM-client
        /// </summary>
        public CiviCrmClient()
        {
            _apiKey = Properties.Settings.Default.UserKey;
            _key = Properties.Settings.Default.SiteKey;
        }

        /// <summary>
        /// Find a contact based on its <paramref name="id"/>
        /// </summary>
        /// <param name="id">contact ID of contact to be found</param>
        /// <returns>Contact with given <paramref name="id"/>, if any. Otherwise <c>null</c>.</returns>
        public Contact ContactGet(int id)
        {
            var contact = base.Channel.ContactGet(_apiKey, _key, id);

            // Cache mapping External ID -> Contact ID, because we might need this lots of times
            // when using an API.

            if (contact != null && !string.IsNullOrEmpty(contact.external_identifier))
            {
                CacheContactId(contact.external_identifier, contact.contact_id);
            }

            return contact;
        }

        /// <summary>
        /// Caches link external ID - contact ID
        /// </summary>
        /// <param name="externalId">External ID</param>
        /// <param name="contactId">Contact ID</param>
        private void CacheContactId(string externalId, int contactId)
        {
            Debug.Assert(!string.IsNullOrEmpty(externalId));
            _cache.Add(String.Format(ContactIdCacheKey, externalId), contactId,
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(2),
                    Priority = CacheItemPriority.Default
                });
        }
    }
}
