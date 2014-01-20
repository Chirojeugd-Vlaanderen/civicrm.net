/*
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

using System;
using System.Collections.Generic;
using Chiro.CiviCrm.Domain;

namespace Chiro.CiviCrm.ClientInterfaces
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
    public interface ICiviCrmClient: IDisposable
    {
        /// <summary>
        /// Find a contact based on its <paramref name="id"/>
        /// </summary>
        /// <param name="id">contact ID of contact to be found</param>
        /// <returns>Contact with given <paramref name="id"/>, if any. Otherwise <c>null</c>.</returns>
        Contact ContactGet(int id);

        /// <summary>
        /// Find a contact based on its <paramref name="externalId"/>.
        /// </summary>
        /// <param name="externalId">External ID of contact to be found</param>
        /// <returns>The contact with given <paramref name="externalId"/>, or <c>null</c> if it is not found.</returns>
        Contact ContactFind(int externalId);

        /// <summary>
        /// Saves a new contact, or updates an existing contact
        /// </summary>
        /// <param name="contact">Contact to be saved or updated</param>
        /// <remarks>If the contact's ID is 0, it will be saved. If it differs from 0, the existing contact with the
        /// given ID will be updated.</remarks>
        void ContactSave(Contact contact);

        /// <summary>
        /// Retrieves the addresses for the contact with given <paramref name="contactId"/>
        /// </summary>
        /// <param name="contactId">ID of the contact whose addresses are to be retrieved</param>
        /// <returns>List of addresses</returns>
        List<Address> ContactAddressesGet(int contactId);
            
        /// <summary>
        /// Retrieves the addresses for the contact with given <paramref name="externalId"/>.
        /// </summary>
        /// <param name="externalId">EXTERNAL ID of the contact whose addresses are to be retrieved</param>
        /// <returns>List of addresses</returns>
        List<Address> ContactAddressesFind(int externalId);

        /// <summary>
        /// Creates a new address, or updates an existing address.
        /// </summary>
        /// <param name="address">Address to be updated (when Id != 0) or saved (when Id == 0).</param>
        void AddressSave(Address address);

        /// <summary>
        /// Delete the address with given <paramref name="addressId"/>.
        /// </summary>
        /// <param name="addressId">ID of the address to be deleted.</param>
        void AddressDelete(int addressId);
    }
}
