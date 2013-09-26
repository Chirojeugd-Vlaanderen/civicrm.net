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

using System.Xml.Serialization;

namespace Chiro.CiviCrm.Domain
{
    public class Address
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("contact_id")]
        public int ContactId { get; set; }

        [XmlElement("location_type_id")]
        public int LocationTypeId { get; set; }

        [XmlElement("is_primary")]
        public bool IsPrimary { get; set; }

        [XmlElement("is_billing")]
        public bool IsBilling { get; set; }

        [XmlElement("street_address")]
        public string StreetAddress { get; set; }

        [XmlElement("city")]
        public string City { get; set; }

        [XmlElement("state_province_id")]
        public int StateProvinceId { get; set; }

        [XmlElement("postal_code")]
        public int PostalCode { get; set; }

        [XmlElement("postal_code_suffix")]
        public string PostalCodeSuffix { get; set; }

        [XmlElement("country_id")]
        public int CountryId { get; set; }
    }
}
