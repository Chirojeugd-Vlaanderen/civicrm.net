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
using System.Xml;
using System.Xml.Serialization;

namespace Chiro.CiviCrm.Domain
{
    public class Contact
    {
        [XmlElement("contact_id")]
        public int Id { get; set; }

        [XmlElement("first_name")]
        public string FirstName { get; set; }

        [XmlElement("last_name")]
        public string LastName { get; set; }

        [XmlElement("external_identifier")]
        public string ExternalId { get; set; }

        [XmlElement("contact_type")]
        public ContactType ContactType { get; set; }

        // the dates don't have the XmlElement attribute, because deserialization happens via the
        // datestrings in Chiro.CiviCrm.Api.DataContracts.ApiContact.
        public DateTime? BirthDate { get; set; }

        public DateTime? DeceasedDate { get; set; }

        // Similarly, the Gender will be derived based on GenderId.
        public Gender Gender { get; set; }

        [XmlElement("is_deceased")]
        public bool IsDeceased { get; set; }
    }
}
