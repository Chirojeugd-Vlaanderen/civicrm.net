﻿/*
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

using System.Security;
using System.Xml.Serialization;

namespace Chiro.CiviCrm.ServiceContracts.DataContracts
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
        public int ExternalId { get; set; }

        [XmlElement("contact_type")]
        public ContactType ContactType { get; set; }
    }
}