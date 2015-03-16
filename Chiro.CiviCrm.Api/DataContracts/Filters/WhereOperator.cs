/*
   Copyright 2015 Chirojeugd-Vlaanderen vzw

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

namespace Chiro.CiviCrm.Api.DataContracts.Filters
{
    /// <summary>
    /// Operator for a where clause.
    /// </summary>
    /// <remarks>
    /// Not all operators are supported yet.
    /// </remarks>
    [DataContract]
    public enum WhereOperator
    {
        /// <summary>
        /// None.
        /// </summary>
        [EnumMember] None,
        /// <summary>
        /// Equal.
        /// </summary>
        [EnumMember] Eq,
        /// <summary>
        /// Less than or equal
        /// </summary>
        [EnumMember] Lte,
        /// <summary>
        /// Greater than or equal
        /// </summary>
        [EnumMember] Gte,
        /// <summary>
        /// Less than
        /// </summary>
        [EnumMember] Lt,
        /// <summary>
        /// Greater than
        /// </summary>
        [EnumMember] Gt,
        /// <summary>
        /// Like
        /// </summary>
        [EnumMember] Like,
        /// <summary>
        /// Not equal
        /// </summary>
        [EnumMember] Ne,
        /// <summary>
        /// Not like
        /// </summary>
        [EnumMember] NotLike,
        /// <summary>
        /// In
        /// </summary>
        [EnumMember] In,
        /// <summary>
        /// Not in
        /// </summary>
        [EnumMember] NotIn,
        /// <summary>
        /// Between
        /// </summary>
        [EnumMember] Between,
        /// <summary>
        /// Not between
        /// </summary>
        [EnumMember] NotBetween,
        /// <summary>
        /// Is not null
        /// </summary>
        [EnumMember] IsNotNull,
        /// <summary>
        /// Is null
        /// </summary>
        [EnumMember] IsNull
    }
}
