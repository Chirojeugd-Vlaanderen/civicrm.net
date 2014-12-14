/*
   Copyright 2014 Chirojeugd-Vlaanderen vzw
   Copyright 2014 Johan Vervloet

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
using Chiro.CiviCrm.Api.DataContracts.Requests;
using Chiro.CiviCrm.Model;
using Chiro.CiviCrm.Model.Requests;
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
            CreateMap<string, int?>().ConvertUsing<NullIntTypeConverter>();
            CreateMap<string, bool?>().ConvertUsing<NullBooleanTypeConverter>();
            CreateMap<string, DateTime?>().ConvertUsing<CiviDateTypeConverter>();

            CreateMap<CiviContact, Contact>()
                .ForMember(
                    dst => dst.ChainedAddresses,
                    opt => opt.MapFrom(src => src.chained_addresses.values));
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
            CreateMap<DateTime?, string>().ConvertUsing<DateCiviTypeConverter>();

            CreateMap<Contact, CiviContact>()
                // Prevent chained entities from being mapped back.
                .ForMember(dst => dst.chained_addresses, opt => opt.Ignore());
            CreateMap<Address, CiviAddress>();

            // Configure mappings for each request.
            // I wonder if this can be done more generic:
            CreateMap<BaseRequest, CiviRequest>()
                .Include<ExternalIdentifierRequest, CiviExternalIdentifierRequest>()
                .Include<IdRequest, CiviIdRequest>()
                .ForMember(src => src.sequential, opt => opt.UseValue(1));
            CreateMap<ExternalIdentifierRequest, CiviExternalIdentifierRequest>();
            CreateMap<IdRequest, CiviIdRequest>();
        }
    }

    /// <summary>
    /// Conversion from string to int?
    /// Thank you http://stackoverflow.com/questions/4101516/automapper-how-to-parse-an-int-from-a-string-and-possible-to-creating-rules-bas
    /// </summary>
    internal class NullIntTypeConverter : TypeConverter<string, int?>
    {
        protected override int? ConvertCore(string source)
        {
            if (source == null)
                return null;
            else
            {
                int result;
                return Int32.TryParse(source, out result) ? (int?)result : null;
            }
        }
    }

    /// <summary>
    /// Conversion from string (null, "", "0" or "1") to bool?
    /// </summary>
    internal class NullBooleanTypeConverter : TypeConverter<string, bool?>
    {
        protected override bool? ConvertCore(string source)
        {
            if (String.IsNullOrEmpty(source))
            {
                return false;
            }
            else
            {
                int result;
                Int32.TryParse(source, out result);
                return (result > 0);
            }
        }
    }

    /// <summary>
    /// Conversion from CiviCRM date (which we get as a string) to a DateTime?
    /// </summary>
    internal class CiviDateTypeConverter : TypeConverter<string, DateTime?>
    {
        protected override DateTime? ConvertCore(string source)
        {
            return String.IsNullOrEmpty(source) ? (DateTime?)null : DateTime.ParseExact(
                source,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Conversion from DateTime? to CiviCRM date.
    /// </summary>
    internal class DateCiviTypeConverter : TypeConverter<DateTime?, string>
    {
        protected override string ConvertCore(DateTime? source)
        {
            return source == null ? null : source.Value.ToString("yyyy-MM-dd");
        }
    }
}
