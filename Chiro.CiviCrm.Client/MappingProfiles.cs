/*
   Copyright 2014 Chirojeugd-Vlaanderen vzw

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

using AutoMapper;
using Chiro.CiviCrm.Api.DataContracts;
using Chiro.CiviCrm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chiro.CiviCrm.Client
{
    /// <summary>
    /// Mapping profile from CiviCRM to .NET
    /// </summary>
    public class CiviToNetProfile : Profile
    {
        protected override void Configure()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<CiviContact, Contact>();
            CreateMap<CiviAddress, Address>();
        }
    };

    /// <summary>
    /// Mapping profile from .NET to CiviCRM.
    /// </summary>
    public class NetToCiviProfile : Profile
    {
        protected override void Configure()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();
            CreateMap<Contact, CiviContact>();
            CreateMap<Address, CiviAddress>();
        }
    }
}
