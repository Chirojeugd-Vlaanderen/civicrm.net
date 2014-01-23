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
            var result = base.Channel.ContactGet(_apiKey, _key, id).Content;

            if (result.Contacts == null)
            {
                return null;
            }

            var contact = result.Contacts.SingleOrDefault();

            if (contact == null)
            {
                return null;
            }

            // Cache mapping External ID -> Contact ID, because we might need this lots of times
            // when using an API.

            if (!string.IsNullOrEmpty(contact.ExternalId))
            {
                CacheContactId(contact.ExternalId, contact.Id);
            }

            return result.Contacts == null ? null : result.Contacts.FirstOrDefault();
        }

        /// <summary>
        /// Find a contact based on its <paramref name="externalId"/>.
        /// </summary>
        /// <param name="externalId">External ID of contact to be found</param>
        /// <returns>The contact with given <paramref name="externalId"/>, or <c>null</c> if it is not found.</returns>
        public Contact ContactFind(string externalId)
        {
            var result = base.Channel.ContactFind(_apiKey, _key, externalId).Content;

            if (result.Contacts == null || result.Contacts.FirstOrDefault() == null)
            {
                return null;
            }

            var contact = result.Contacts.First();

            CacheContactId(externalId, contact.Id);

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
            Channel.ContactSave(_apiKey, _key, contact.Id, contact.FirstName, contact.LastName, contact.ExternalId,
                contact.ContactType, contact.BirthDate, contact.DeceasedDate, Convert.ToInt32(contact.IsDeceased), (int)(contact.Gender));
        }

        /// <summary>
        /// Retrieves the addresses for the contact with given <paramref name="contactId"/>
        /// </summary>
        /// <param name="contactId">ID of the contact whose addresses are to be retrieved</param>
        /// <returns>List of addresses</returns>
        public List<Address> ContactAddressesGet(int contactId)
        {
            var result = Channel.ContactAddressesGet(_apiKey, _key, contactId).Content;

            if (result == null || result.Adresses == null)
            {
                return new List<Address>();
            }

            return result.Adresses.ToList();
        }

        /// <summary>
        /// Retrieves the addresses for the contact with given <paramref name="externalId"/>.
        /// </summary>
        /// <param name="externalId">EXTERNAL ID of the contact whose addresses are to be retrieved</param>
        /// <returns>List of addresses</returns>
        public List<Address> ContactAddressesFind(string externalId)
        {
            int? contactId = ExternalIdToContactId(externalId);

            if (contactId == null)
            {
                return new List<Address>();
            }

            var result = Channel.ContactAddressesGet(_apiKey, _key, contactId.Value).Content;

            if (result == null || result.Adresses == null)
            {
                return new List<Address>();
            }

            return result.Adresses.ToList();
        }

        /// <summary>
        /// Returns the contact ID of the contact with given <paramref name="externalId" />. Caches.
        /// </summary>
        /// <param name="externalId">An external ID of a contact</param>
        /// <returns>The corresponding contact ID, or <c>null</c> if not found.</returns>
        private int? ExternalIdToContactId(string externalId)
        {
            Debug.Assert(!String.IsNullOrEmpty(externalId));
            int? contactId = (int?)_cache.Get(String.Format(ContactIdCacheKey, externalId));

            if (contactId == null)
            {
                var contact = ContactFind(externalId);
                if (contact == null)
                {
                    return null;
                }
                contactId = contact.Id;
                CacheContactId(externalId, contactId.Value);
            }

            return contactId;
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

        /// <summary>
        /// Creates a new address, or updates an existing address.
        /// </summary>
        /// <param name="address">Address to be updated (when Id != 0) or saved (when Id == 0).</param>
        public void AddressSave(Address address)
        {
            Channel.AddressSave(_apiKey, _key, address.Id, address.ContactId, address.LocationTypeId, Convert.ToInt32(address.IsPrimary),
                Convert.ToInt32(address.IsBilling), address.StreetAddress, address.City, address.StateProvinceId, address.PostalCode,
                address.PostalCodeSuffix, address.Country);
        }

        /// <summary>
        /// Delete the address with given <paramref name="addressId"/>.
        /// </summary>
        /// <param name="addressId">ID of the address to be deleted.</param>
        public void AddressDelete(int addressId)
        {
            Channel.AddressDelete(_apiKey, _key, addressId);
        }
    }
}
