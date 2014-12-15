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
using Chiro.CiviCrm.Model;
using Chiro.CiviCrm.Model.Requests;

namespace Chiro.CiviCrm.ClientInterfaces
{
    /// <summary>
    /// Interface for the CiviCRM-client.
    /// </summary>
    public interface ICiviCrmClient: IDisposable
    {
        /// <summary>
        /// Find a single contact based on the <paramref name="request"/>
        /// </summary>
        /// <param name="request">search criteria for the contact</param>
        /// <returns>If found, the unique contact statisfying the <paramref name="request"/>.</returns>
        Contact ContactGetSingle(BaseRequest request);

        /// <summary>
        /// Find contacts satisifying the <paramref name="request"/>.
        /// </summary>
        /// <param name="request">search criteria for the contact</param>
        /// <returns>The contacts statisfying the <paramref name="request"/>.</returns>
        Contact[] ContactGet(BaseRequest request);

        /// <summary>
        /// Creates or updates the <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact to be saved. If it has an ID, the existing contat will be overwritten.
        /// Otherwise a new contact is created.</param>
        /// <param name="options">Options to pass to the API.</param>
        /// <returns>The saved contact, with ID.</returns>
        Contact ContactSave(Contact contact, ApiOptions options);

        /// <summary>
        /// Creates or updates the given <paramref name="address"/>.
        /// </summary>
        /// <param name="address">Address to be saved.</param>
        /// <param name="options">Options to pass to the API.</param>
        /// <returns>The saved address.</returns>
        Address AddressSave(Address address, ApiOptions options);

        /// <summary>
        /// Deletes the address with given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">AddressId of address to be deleted.</param>
        void AddressDelete(int id);
    }
}
