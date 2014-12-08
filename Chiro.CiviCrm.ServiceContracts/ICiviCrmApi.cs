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
using System.ServiceModel;
using System.ServiceModel.Web;
using Chiro.CiviCrm.Api.DataContracts;

namespace Chiro.CiviCrm.Api
{
    /// <summary>
    /// WCF service contract for the CiviCRM API
    /// </summary>
    /// <remarks>
    /// I use CiviCrmResponse as the result type, because I had problems with deserializing because the root element
    /// of every response of the CiviCRM API is ResultSet. WCF threw an exception because it didn't want to deserialize
    /// the same XML element to different data contracts.
    /// I used the workaround I found here:
    /// http://social.msdn.microsoft.com/Forums/vstudio/en-US/bcd031d7-c8a4-4bb0-8c85-bc5d7b46108a/rest-services-identical-xmlroot-attributes-on-different-classes
    /// but I am not sure whether this solution is OK.
    /// </remarks>
    [ServiceContract]
    public interface ICiviCrmApi
    {
        /// <summary>
        /// Find contact with given <paramref name="id"/>
        /// </summary>
        /// <param name="key">Key of the CiviCRM-instance</param>
        /// <param name="id">External ID of contact to be found</param>
        /// <param name="apiKey">API-key of the API-user</param>
        /// <returns>If found, a set with the (unique) contact with 
        /// given <paramref name="id"/>,
        /// otherwise <c>null</c>.</returns>
        /// <remarks>To avoid problems with the query string formatter, I try
        /// id={id}&json=1 first. If I can get it to work, I will use the
        /// syntax that is provided by the API explorer later on:
        /// json={id:{id}}. (Which will obviously require more tweaking.)</remarks>
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
            UriTemplate =
                "?api_key={apiKey}&key={key}&debug=1&version=3&entity=Contact&action=getsingle&json={id}"
            )]
        CiviContact ContactGet(string apiKey, string key, CiviId id);
    }
}
