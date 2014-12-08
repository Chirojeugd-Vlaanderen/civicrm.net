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

            Mapper.Initialize(cfg => { 
                cfg.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
                cfg.CreateMap<CiviContact, Contact>();
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
        public Contact ContactFind(int externalIdentifier)
        {
            var civiContact = base.Channel.ContactFind(_apiKey, _key, new CiviExternalIdentifier(externalIdentifier));
            return Mapper.Map<Contact>(civiContact);
        }
    }
}
