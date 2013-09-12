﻿/*
   Copyright 2013 Chirojeugd-Vlaanderen vzw

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
using System.ServiceModel;
using Chiro.CiviCrm.ClientInterfaces;
using Chiro.CiviCrm.ServiceContracts;
using Chiro.CiviCrm.ServiceContracts.DataContracts;

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

        /// <summary>
        /// Creates a new CiviCRM-client
        /// </summary>
        public CiviCrmClient()
        {
            _apiKey = Properties.Settings.Default.UserKey;
            _key = Properties.Settings.Default.SiteKey;
        }

        /// <summary>
        /// Find a contact based on its <paramref name="externalId"/>.
        /// </summary>
        /// <param name="externalId">External ID of contact to be found</param>
        /// <returns>The contact with given <paramref name="externalId"/>, or <c>null</c> if it is not found.</returns>
        public Contact ContactFind(int externalId)
        {
            var result = base.Channel.ContactFind(_apiKey, _key, externalId);

            return result.Contacts == null ? null : result.Contacts.FirstOrDefault();
        }

        /// <summary>
        /// Saves a new contact, or updates an existing contact
        /// </summary>
        /// <param name="contact">Contact to be saved or updated</param>
        /// <remarks>If the contact's ID is 0, it will be saved. If it differs from 0, the existing contact with the
        /// given ID will be updated.</remarks>
        public void ContactSave(Contact contact)
        {
            Channel.ContactSave(_apiKey, _key, contact.Id, contact.FirstName, contact.LastName, contact.ExternalId, contact.ContactType);
        }
    }
}
