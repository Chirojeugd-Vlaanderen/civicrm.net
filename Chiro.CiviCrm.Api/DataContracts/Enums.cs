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
    [DataContract]
    public enum CiviEntity
    {
        [EnumMember] Contact,
        [EnumMember] Address,
        [EnumMember] Phone,
        [EnumMember] Email,
        [EnumMember] Website,
        [EnumMember] Im,
        [EnumMember] Relationship,
    }

    [DataContract]
    public enum ApiAction
    {
        [EnumMember] Create,
        [EnumMember] Get,
        [EnumMember] GetSingle
    }

    [DataContract]
    public enum ContactType
    {
        [EnumMember] Individual,
        [EnumMember] Organization,
        [EnumMember] Household
    }

    [DataContract]
    public enum MembershipStatus
    {
        [EnumMember] New,
        [EnumMember] Current,
        [EnumMember] Grace,
        [EnumMember] Expired,
        [EnumMember] Pending,
        [EnumMember] Cancelled,
        [EnumMember] Deceased
    }

    [DataContract]
    public enum CommunicationMethod
    {
        [EnumMember] Phone = 1,
        [EnumMember] Email = 2,
        [EnumMember] PostalMail = 3,
        [EnumMember] Sms = 4,
        [EnumMember] Fax = 5
    }

    [DataContract]
    public enum MailFormat
    {
        [EnumMember] Both,
        [EnumMember] HTML, // Dont change casing, API probably needs it this way.
        [EnumMember] Text
    }

    [DataContract]
    public enum Gender
    {
        [EnumMember] Female = 1,
        [EnumMember] Male = 2,
        [EnumMember] Transgender = 3
    }

    [DataContract]
    public enum CommunicationStyle
    {
        [EnumMember] Formal = 1,
        [EnumMember] Familiar = 2
    }

    // The enum members below are actually configurable in CiviCRM. So you might need to
    // change them. (Or better: find a better way to deal with this.)

    [DataContract]
    public enum PhoneType
    {
        [EnumMember] Phone = 1,
        [EnumMember] Mobile = 2,
        [EnumMember] Fax = 3,
        [EnumMember] Pager = 4,
        [EnumMember] Voicemail = 5
    }


    // Remember the nillies :-)
    [DataContract]
    public enum Provider
    {
        [EnumMember] Yahoo = 1,
        [EnumMember] Msn = 2,
        [EnumMember] Aim = 3,
        [EnumMember] GTalk = 4,
        [EnumMember] Jabber = 5,
        [EnumMember] Skype = 6
    }

    [DataContract]
    public enum WebsiteType
    {
        [EnumMember] Work = 1,
        [EnumMember] Main = 2,
        [EnumMember] Facebook = 3,
        [EnumMember] GooglePlus = 4,
        [EnumMember] Instagram = 5,
        [EnumMember] LinkedIn = 6,
        [EnumMember] MySpace = 7,
        [EnumMember] Pinterest = 8,
        [EnumMember] SnapChat = 9,
        [EnumMember] Tumblr = 10,
        [EnumMember] Twitter = 11,
        [EnumMember] Vine = 12
    }
}
