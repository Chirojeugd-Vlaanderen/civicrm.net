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
using Chiro.CiviCrm.Domain;

namespace Chiro.CiviCrm.ClientInterfaces
{
    /// <summary>
    /// Interface for the CiviCRM-client.
    /// </summary>
    public interface ICiviCrmClient: IDisposable
    {
        /// <summary>
        /// Find a contact based on its <paramref name="id"/>
        /// </summary>
        /// <param name="id">contact ID of contact to be found</param>
        /// <returns>Contact with given <paramref name="id"/>, if any. Otherwise <c>null</c>.</returns>
        Contact ContactGet(int id);

        /// <summary>
        /// Find a contact with given <paramref name="externalIdentifier"/>.
        /// </summary>
        /// <param name="externalIdentifier">External identifier of requested contact.</param>
        /// <returns>Contact with given <paramref name="externalIdentifier"/>, if any.
        /// <c>null</c> otherwise.</returns>
        Contact ContactFind(string externalIdentifier);

        /// <summary>
        /// Creates or updates the <paramref name="contact"/>.
        /// </summary>
        /// <param name="contact">Contact to be saved. If it has an ID, the existing contat will be overwritten.
        /// Otherwise a new contact is created.</param>
        /// <returns>The saved contact, with ID.</returns>
        Contact ContactSave(Contact contact);

        /// <summary>
        /// Returns the adresses of the contact with given <paramref name="contactId"/>.
        /// </summary>
        /// <param name="contactId">ID of the contact whose adresses you are requesting.</param>
        /// <returns>Adresses of the contact with given <paramref name="contactId"/></returns>
        Address[] ContactAddressesGet(int contactId);

        /// <summary>
        /// Creates or updates the given <paramref name="address"/>.
        /// </summary>
        /// <param name="address">Address to be saved.</param>
        /// <returns>The saved address.</returns>
        Address AddressSave(Address address);
    }
}
