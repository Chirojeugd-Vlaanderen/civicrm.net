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
using System.Text;

namespace Chiro.CiviCrm.Domain
{
    public class Address
    {
        public int? Id { get; set; }
        public int? ContactId { get; set; }
        public int LocationTypeId { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsBilling { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int? CountryId { get; set; }
        /// <summary>
        /// Name of country, or ISO-code
        /// </summary>
        /// <remarks>
        /// You can use this to create/update the country of an address.
        /// The CiviCRM address API doesn't seem to fetch the country.
        /// </remarks>
        public string Country { get;  set; }
    }
}
