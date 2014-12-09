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

namespace Chiro.CiviCrm.Domain
{
    public class Contact
    {
        public int? Id { get; set; }
        public ContactType ContactType { get; set; }
        public string ContactSubType { get; set; }
        public string SortName { get; set; }
        public string DisplayName { get; set; }
        public bool DoNotEmail { get; set; }
        public bool DoNotPhone { get; set; }
        public bool DoNotSms { get; set; }
        public bool DoNotTrade { get; set; }
        public bool IsOptOut { get; set; }
        public string LegalIdentifier { get; set; }
        public string ExternalIdentifier { get; set; }
        public string NickName { get; set; }
        public string LegalName { get; set; }
        public string ImageUrl { get; set; }
        //public CommunicationMethod[] PreferredCommunicationMethod { get; set; }
        public string PreferredLanguage { get; set; }
        public MailFormat PreferredMailFormat { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FormalTitle { get; set; }
        public CommunicationStyle CommunicationStyleId { get; set; }
        public string JobTitle { get; set; }
        public Gender GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsDeceased { get; set; }
        public DateTime? DeceasedDate { get; set; }
        public string HouseholdName { get; set; }
        public string OrganizationName { get; set; }
        public string SicCode { get; set; }
        public bool ContactIsDeleted { get; set; }
        public string CurrentEmployer { get; set; }
        public int? AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string SupplementalAddress1 { get; set; }
        public string SupplementalAddress2 { get; set; }
        public string City { get; set; }
        public string PostalCodeSuffix { get; set; }
        public string PostalCode { get; set; }
        public string GeoCode1 { get; set; }
        public string GeoCode2 { get; set; }
        public int? PhoneId { get; set; }
        public PhoneType? PhoneTypeId { get; set; }
        public string Phone { get; set; }
        public int? EmailId { get; set; }
        public string Email { get; set; }
        public bool? OnHold { get; set; }
        public int? ImId { get; set; }
        public Provider? ProviderId { get; set; }
        public string Im { get; set; }
        public string WorldRegion { get; set; }
        public string IndividualPrefix { get; set; }
        public string IndividualSuffix { get; set; }
        public string StateProvinceName { get; set; }
        public string Country { get; set; }
    }
}
