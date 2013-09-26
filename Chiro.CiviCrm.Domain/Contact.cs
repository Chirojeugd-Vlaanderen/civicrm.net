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
using System.Xml;
using System.Xml.Serialization;

namespace Chiro.CiviCrm.Domain
{
    public class Contact
    {
        #region Ugly hack

        // I need this to handle null DateTimes returned by the CiviCRM API.
        // Maybe this can be avoided using a client side custom MessageFormatter.
        // If not, these 'DateStrings' should not be visible to the user of
        // Chiro.CiviCrm.Client.

        [XmlElement("birth_date")]
        public string BirthDateString
        {
            get
            {
                return BirthDate.HasValue
                    ? XmlConvert.ToString(BirthDate.Value, XmlDateTimeSerializationMode.Unspecified)
                    : String.Empty;
            }
            set
            {
                BirthDate = string.IsNullOrEmpty(value)
                    ? (DateTime?) null
                    : XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified);
            }
        }

        [XmlElement("deceased_date")]
        public string DeceasedDateString
        {
            get
            {
                return DeceasedDate.HasValue
                    ? XmlConvert.ToString(DeceasedDate.Value, XmlDateTimeSerializationMode.Unspecified)
                    : String.Empty;
            }
            set
            {
                DeceasedDate = string.IsNullOrEmpty(value)
                    ? (DateTime?)null
                    : XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Unspecified);
            }
        }

        #endregion

        [XmlElement("contact_id")]
        public int Id { get; set; }

        [XmlElement("first_name")]
        public string FirstName { get; set; }

        [XmlElement("last_name")]
        public string LastName { get; set; }

        [XmlElement("external_identifier")]
        public int? ExternalId { get; set; }

        [XmlElement("contact_type")]
        public ContactType ContactType { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? DeceasedDate { get; set; }

        [XmlElement("is_deceased")]
        public bool IsDeceased { get; set; }

        [XmlElement("gender")]
        public Gender Gender { get; set; }

        [XmlElement("gender_id")]
        public int GenderId { get; set; }
    }
}
