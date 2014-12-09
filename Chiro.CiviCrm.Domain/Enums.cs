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

namespace Chiro.CiviCrm.Domain
{
    public enum ContactType
    {
        Individual,
        Organization,
        Household
    }

    public enum CommunicationMethod
    {
        Phone = 1,
        Email = 2,
        PostalMail = 3,
        Sms = 4,
        Fax = 5
    }

    public enum MailFormat
    {
        Both,
        HTML,
        Text
    }

    public enum Gender
    {
        Female = 1,
        Male = 2,
        Transgender = 3
    }

    public enum CommunicationStyle
    {
        Formal = 1,
        Familiar = 2
    }

    public enum PhoneType
    {
        Phone = 1,
        Mobile = 2,
        Fax = 3,
        Pager = 4,
        Voicemail = 5
    }

    // Remember the nillies :-)

    public enum Provider
    {
        Yahoo = 1,
        MSN = 2,
        AIM = 3,
        GTalk = 4,
        Jabber = 5,
        Facebook = 6
    }
}
