﻿/*
   Copyright 2015,2016 Chirojeugd-Vlaanderen vzw

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
using Chiro.CiviCrm.Api.DataContracts;
using System.ServiceModel;
using System.ServiceModel.Web;
using Chiro.CiviCrm.Api.DataContracts.Requests;

namespace Chiro.CiviCrm.Api
{
    // Custom actions on the CiviCRM API.
    public partial interface ICiviCrmApi
    {
        /// <summary>
        /// Haalt combinaties stamnummer-adnummer op voor alle actieve leden.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="key"></param>
        /// <param name="request">Dient vooral om Limit en Offset mee te geven.</param>
        /// <returns></returns>
		[OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=ChiroDiagnostics&action=getactievelidrelaties&json={request}&sequential=1")]
        ApiResultStrings ChiroDiagnosticsActieveLidRelaties(string apiKey, string key, BaseRequest request);
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "?api_key={apiKey}&key={key}&debug=1&version=3&entity=ChiroDiagnostics&action=getmembersverzekerdloonverlies&json=1&sequential=1")]
        ApiResultStrings ChiroDiagnosticsMembersVerzekerdLoonVerlies(string apiKey, string key);
    }
}
