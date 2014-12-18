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

using System.Runtime.Serialization;
namespace Chiro.CiviCrm.Api.DataContracts
{
    public enum CiviEntity
    {
        Contact,
        Address
        // TODO: support more entities
    }

    [DataContract]
    public enum ContactType
    {
        [EnumMember]
        Individual,
        [EnumMember]
        Organization,
        [EnumMember]
        Household
    }

    [DataContract]
    public enum CommunicationMethod
    {
        [EnumMember]
        Phone = 1,
        [EnumMember]
        Email = 2,
        [EnumMember]
        PostalMail = 3,
        [EnumMember]
        Sms = 4,
        [EnumMember]
        Fax = 5
    }

    [DataContract]
    public enum MailFormat
    {
        [EnumMember]
        Both,
        [EnumMember]
        HTML,
        [EnumMember]
        Text
    }

    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Female = 1,
        [EnumMember]
        Male = 2,
        [EnumMember]
        Transgender = 3
    }

    [DataContract]
    public enum CommunicationStyle
    {
        [EnumMember]
        Formal = 1,
        [EnumMember]
        Familiar = 2
    }

    [DataContract]
    public enum PhoneType
    {
        [EnumMember]
        Phone = 1,
        [EnumMember]
        Mobile = 2,
        [EnumMember]
        Fax = 3,
        [EnumMember]
        Pager = 4,
        [EnumMember]
        Voicemail = 5
    }

    // Remember the nillies :-)
    [DataContract]
    public enum Provider
    {
        [EnumMember]
        Yahoo = 1,
        [EnumMember]
        MSN = 2,
        [EnumMember]
        AIM = 3,
        [EnumMember]
        GTalk = 4,
        [EnumMember]
        Jabber = 5,
        [EnumMember]
        Facebook = 6
    }
}
