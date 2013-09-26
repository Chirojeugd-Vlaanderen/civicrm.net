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

using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Hack to work around the problem that the CiviCRM API puts all replies in
    /// a node of type 'ResultSet', containing nodes of type 'Result'. WCF
    /// does not like this. Thanks to Renaud Bedard for the workaround:
    /// http://social.msdn.microsoft.com/Forums/vstudio/en-US/bcd031d7-c8a4-4bb0-8c85-bc5d7b46108a/rest-services-identical-xmlroot-attributes-on-different-classes
    /// </summary>
    /// <typeparam name="T">Expected type of the result</typeparam>
    [XmlSchemaProvider(null, IsAny = true)]
    public class CiviCrmResponse<T>: IXmlSerializable
    {
        readonly XmlSerializer _contentSerializer = new XmlSerializer(typeof(T));

        public T Content { get; internal set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Content = (T) _contentSerializer.Deserialize(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotSupportedException();
        }
    }
}
