/*
   Copyright 2013, 2014 Chirojeugd-Vlaanderen vzw
   Copyright 2015 Johan Vervloet

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
using Chiro.CiviCrm.Model;
using System.Diagnostics;
using Chiro.CiviCrm.Api.DataContracts;
using AutoMapper;
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviCrm.Model.Requests;
using AutoMapper.Mappers;

namespace Chiro.CiviCrm.Client
{
    /// <summary>
    /// Interface for the CiviCRM-client.
    /// </summary>
    public class CiviCrmClient: ClientBase<ICiviCrmApi>, ICiviCrmClient
    {
        private string _apiKey;
        private string _key;

        private static MappingEngine _mapper = null;

        /// <summary>
        /// Create a private mapping engine, to not disturb possible
        /// existing automapper configurations.
        /// </summary>
        private void ConfigureMapper()
        {
            // See issue #20 (https://github.com/johanv/civicrm.net/issues/20).

            var store = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            store.AssertConfigurationIsValid();
            _mapper = new MappingEngine(store);

            store.AddProfile<CiviToNetProfile>();
            store.AddProfile<NetToCiviProfile>();
        }

        /// <summary>
        /// Creates a new CiviCRM-client
        /// </summary>
        public CiviCrmClient()
        {
            if (_mapper == null)
            {
                ConfigureMapper();
            }
        }

        /// <summary>
        /// Create a a new CiviCRM-client, and configure it with the given
        /// <paramref name="siteKey"/> and <paramref name="apiKey"/>.
        /// </summary>
        /// <param name="siteKey">Site key of your CiviCRM instance.</param>
        /// <param name="apiKey">API key of an API user in your CiviCRM instance.</param>
        public CiviCrmClient(string siteKey, string apiKey):this()
        {
            Configure(siteKey, apiKey);
        }

        /// <summary>
        /// Configure the CiviCRM client.
        /// </summary>
        /// <param name="siteKey">Site key of your CiviCRM instance.</param>
        /// <param name="apiKey">API key of a user with CiviCRM API access.</param>
        public void Configure(string siteKey, string apiKey)
        {
            _apiKey = apiKey;
            _key = siteKey;
        }

        /// <summary>
        /// Find a single contact based on the <paramref name="request"/>
        /// </summary>
        /// <param name="request">search criteria for the contact</param>
        /// <returns>If found, the unique contact statisfying the <paramref name="request"/>.</returns>
        public Contact ContactGetSingle(BaseRequest request)
        {
            var civiContact = base.Channel.ContactGetSingle(_apiKey, _key, _mapper.Map<CiviRequest>(request));

            return _mapper.Map<Contact>(civiContact);
        }

        /// <summary>
        /// Find contacts satisifying the <paramref name="request"/>.
        /// </summary>
        /// <param name="request">search criteria for the contact</param>
        /// <returns>The contacts statisfying the <paramref name="request"/>.</returns>
        public Contact[] ContactGet(BaseRequest request)
        {
            var result = base.Channel.ContactGet(_apiKey, _key, _mapper.Map<CiviRequest>(request));
            return _mapper.Map<Contact[]>(result.values);
        }


        /// <summary>
        /// Creates or updates the <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact to be saved. If it has an ID, the existing contat will be overwritten.
        /// Otherwise a new contact is created.</param>
        /// <param name="options">Options to pass to the API.</param>
        /// <returns>The saved contact, with ID.</returns>
        public Contact ContactSave(Contact contact, ApiOptions options)
        {
            var civiContact = _mapper.Map<CiviContact>(contact);
            if (options != null)
            {
                civiContact.options = _mapper.Map<CiviApiOptions>(options);
            }

            // If you get a mapping exception here regarding mapping empty strings too booleans,
            // you are probably saving a contact that has an invalid contact id.

            var result = base.Channel.ContactSave(_apiKey, _key, civiContact);
            AssertValid(result);
            return _mapper.Map<Contact>(result.values.FirstOrDefault());
        }

        /// <summary>
        /// Creates or updates the given <paramref name="address"/>.
        /// </summary>
        /// <param name="address">Address to be saved.</param>
        /// <param name="options">Options to pass to the API.</param>
        /// <returns>The saved address.</returns>
        public Address AddressSave(Address address, ApiOptions options)
        {
            var civiAddress = _mapper.Map<CiviAddress>(address);
            if (options != null)
            {
                civiAddress.options = _mapper.Map<CiviApiOptions>(options);
            }

            var result = base.Channel.AddressSave(_apiKey, _key, civiAddress);
            AssertValid(result);
            return _mapper.Map<Address>(result.values.FirstOrDefault());
        }

        /// <summary>
        /// Deletes the address with given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">AddressId of address to be deleted.</param>
        public void AddressDelete(int id)
        {
            var result = base.Channel.AddressDelete(_apiKey, _key, new CiviIdRequest(id));
            AssertValid(result);
        }

        /// <summary>
        /// Throws an exception of the <paramref name="result"/> of a CiviCRM API call
        /// is an error.
        /// </summary>
        /// <typeparam name="T">Type of the entities in <paramref name="result"/>.value</typeparam>
        /// <param name="result">The CiviCRM API result to check.</param>
        private static void AssertValid(CiviResult result)
        {
            if (result.is_error > 0)
            {
                throw new InvalidOperationException(result.error_message);
            }
        }
    }
}
