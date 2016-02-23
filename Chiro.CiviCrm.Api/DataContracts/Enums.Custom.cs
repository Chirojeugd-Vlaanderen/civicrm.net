/*
   Copyright 2015, 2016 Chirojeugd-Vlaanderen vzw

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
using System.Runtime.Serialization;

namespace Chiro.CiviCrm.Api.DataContracts
{
    public enum Afdeling
    {
        Ribbels = 'P',
        Speelclub = 'S',
        Rakwis = 'R',
        Titos = 'T',
        Ketis = 'K',
        Aspis = 'A',
        Leiding = 'L',
        Speciaal = 'X'
    }

    public enum RelatieType
    {
        WerknemerBij = 5,
        GezinshoofdVan = 7,
        GezinslidVan = 8,
        BovenliggendePloegVan = 11,
        OpvolgerVan = 12,
        LidVan = 13,
        HeeftFunctieBijNationaal = 14,
        StagebegeleiderVan = 15
    }

    public enum MembershipType
    {
        Aansluiting = 1,
        DubbelpuntAbonnement = 2
    }

    public enum EvenementType
    {
        Bivak = 100,
        Sb = 1,
        Afdelingsboost = 2,
        Tochtenbivak = 3,
        Groepsleidingsweekend1 = 4,
        Groepsleidingsweekend2 = 5,
        Prikkel = 6,
        Expressiebivak = 7,
        Kic1 = 8,
        Kic2 = 9,
        Kic3 = 10,
        VoortgezetteKadervorming = 11,
        Atd = 12,
        K3d = 17,
        VbDag = 15,
        RegionaleStartdag = 16,
        Ik = 19,
        NationaleStartdag = 22,
        KrinkelWeekend = 25,
        ChiroScoutsCursus = 20,
        Aspitrant = 23,
        Techniekendag = 26,
        Hoofdanimator = 27,
        Krinkel = 28,
        Expressieweekend = 29,
        Jbc = 30,
        Workshopweekend = 31,
        Aspibivak = 32,
        Animatorcursus = 33,
        Werkweek = 101
    }

    [DataContract]
    public enum FactuurStatus
    {
        [EnumMember] VolledigTeFactureren = 1,
        [EnumMember] ExtraVerzekeringTeFactureren = 2,
        [EnumMember] FactuurOk = 3
    }

    [DataContract]
    public enum KaderNiveau
    {
        [EnumMember] PlaatselijkeGroep = 2,
        [EnumMember] Gewest = 4,
        [EnumMember] Verbond = 6,
        [EnumMember] NationalePloeg = 8
    }

    [DataContract]
    [Flags]
    public enum AbonnementType
    {
        [EnumMember] Digitaal = 1,
        [EnumMember] Papier = 2,
        [EnumMember] FullOption = Digitaal|Papier
    }
}
