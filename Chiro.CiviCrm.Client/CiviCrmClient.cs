﻿/*
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
using System.ServiceModel;
using Chiro.CiviCrm.Api;
using Chiro.CiviCrm.ClientInterfaces;
using Chiro.CiviCrm.Domain;
using System.Diagnostics;
using Chiro.CiviCrm.Api.DataContracts;
using AutoMapper;

namespace Chiro.CiviCrm.Client
{
    /// <summary>
    /// Interface for the CiviCRM-client.
    /// </summary>
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

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CiviToNetProfile>();
                cfg.AddProfile<NetToCiviProfile>();
            });
        }

        /// <summary>
        /// Find a contact based on its <paramref name="id"/>
        /// </summary>
        /// <param name="id">contact ID of contact to be found</param>
        /// <returns>Contact with given <paramref name="id"/>, if any. Otherwise <c>null</c>.</returns>
        public Contact ContactGet(int id)
        {
            var civiContact = base.Channel.ContactGet(_apiKey, _key, new CiviId(id));
            return Mapper.Map<Contact>(civiContact);
        }

        /// <summary>
        /// Find a contact with given <paramref name="externalIdentifier"/>.
        /// </summary>
        /// <param name="externalIdentifier">External identifier of requested contact.</param>
        /// <returns>Contact with given <paramref name="externalIdentifier"/>, if any.
        /// <c>null</c> otherwise.</returns>
        public Contact ContactFind(string externalIdentifier)
        {
            var civiContact = base.Channel.ContactFind(_apiKey, _key, new CiviExternalIdentifier(externalIdentifier));
            return Mapper.Map<Contact>(civiContact);
        }


        /// <summary>
        /// Creates or updates the <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact to be saved. If it has an ID, the existing contat will be overwritten.
        /// Otherwise a new contact is created.</param>
        /// <returns>The saved contact, with ID.</returns>
        public Contact ContactSave(Contact contact)
        {
            var civiContact = Mapper.Map<CiviContact>(contact);
            var result = base.Channel.ContactSave(_apiKey, _key, civiContact);
            AssertValid(result);
            return Mapper.Map<Contact>(result.values.FirstOrDefault());
        }

        /// <summary>
        /// Returns the adresses of the contact with given <paramref name="contactId"/>.
        /// </summary>
        /// <param name="contactId">ID of the contact whose adresses you are requesting.</param>
        /// <returns>Adresses of the contact with given <paramref name="contactId"/></returns>
        public Address[] ContactAddressesGet(int contactId)
        {
            var result = base.Channel.ContactAdressesGet(_apiKey, _key, new CiviContactId(contactId));
            AssertValid(result);
            return Mapper.Map<Address[]>(result.values);
        }

        /// <summary>
        /// Creates or updates the given <paramref name="address"/>.
        /// </summary>
        /// <param name="address">Address to be saved.</param>
        /// <returns>The saved address.</returns>
        public Address AddressSave(Address address)
        {
            var civiAddress = Mapper.Map<CiviAddress>(address);

            var result = base.Channel.AddressSave(_apiKey, _key, civiAddress);
            AssertValid(result);
            return Mapper.Map<Address>(result.values.FirstOrDefault());
        }

        /// <summary>
        /// Throws an exception of the <paramref name="result"/> of a CiviCRM API call
        /// is an error.
        /// </summary>
        /// <typeparam name="T">Type of the entities in <paramref name="result"/>.value</typeparam>
        /// <param name="result">The CiviCRM API result to check.</param>
        private static void AssertValid<T>(CiviResult<T> result)
        {
            if (result.is_error > 0)
            {
                throw new InvalidOperationException(result.error_message);
            }
        }
    }
}
