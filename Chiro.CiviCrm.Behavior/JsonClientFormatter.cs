/*
   Based on code of Carlus Figueira
   http://blogs.msdn.com/b/endpoint/archive/2011/05/03/wcf-extensibility-message-formatters.aspx
  
   Copyright 2013-2015 Chirojeugd-Vlaanderen vzw

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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using Chiro.CiviCrm.Api.DataContracts;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.BehaviorExtension
{
    class JsonClientFormatter : IClientMessageFormatter
    {
        readonly OperationDescription _operation;
        readonly Uri _operationUri;

        private static readonly Regex ValuesObjectInsteadOfArrayExpression = new Regex("\"values\":[{]");
        private static readonly Regex KeyValueArrayItemExpression = new Regex("\"[0-9]+\":[{]");

        public JsonClientFormatter(OperationDescription operation, ServiceEndpoint endpoint)
        {
            this._operation = operation;
            string endpointAddress = endpoint.Address.Uri.ToString();
            if (!endpointAddress.EndsWith("/"))
            {
                endpointAddress = endpointAddress + "/";
            }

            this._operationUri = new Uri(endpointAddress + operation.Name);
        }

        public object DeserializeReply(Message message, object[] parameters)
        {
            object bodyFormatProperty;
            if (!message.Properties.TryGetValue(WebBodyFormatMessageProperty.Name, out bodyFormatProperty) ||
                (bodyFormatProperty as WebBodyFormatMessageProperty).Format != WebContentFormat.Raw)
            {
                throw new InvalidOperationException("Incoming messages must have a body format of Raw. Is a ContentTypeMapper set on the WebHttpBinding?");
            }

            var bodyReader = message.GetReaderAtBodyContents();
            var serializer = new JsonSerializer();
            bodyReader.ReadStartElement("Binary");
            byte[] body = bodyReader.ReadContentAsBase64();

            // I tried to use a converter for this. But I failed miserably :-)
            body = WorkAroundCrm15891(body);

            using (var ms = new MemoryStream(body))
            {
                using (var sr = new StreamReader(ms))
                {
                    Type returnType = this._operation.Messages[1].Body.ReturnValue.Type;
                    if (returnType == typeof (EmptyResult))
                    {
                        // If the result of the API cannot be parsed, you can use EmptyResult
                        // as a result. In this case the body is ignored.
                        return new EmptyResult();
                    }
                    var result = serializer.Deserialize(sr, returnType);
                    return result;
                }
            }
        }

        private static byte[] WorkAroundCrm15891(byte[] body)
        {
            // If you chain an 'api.create' with multiple entities of the same type, the resulting Json cannot
            // be parsed by the serializer. See this thread on the civicrm forums:
            // http://forum.civicrm.org/index.php/topic,35393.0.html
            // https://issues.civicrm.org/jira/browse/CRM-15891

            string bodyString = Encoding.UTF8.GetString(body);

            // If the request contained a chained call with an array,
            // the result doesn't return an array in 'values', but
            // a key-value pair. And 'api.entity.create' results are
            // in an object with the id of the main object as key.
            // We will try to detect this kind of key-value-pairs, and replace
            // them by ordinary arrays, so that our JSON parser won't complain.

            // We can recognize this kind of key-value-pairs by a numerical key
            // and an object as value.
            while (KeyValueArrayItemExpression.IsMatch(bodyString))
            {
                int index = KeyValueArrayItemExpression.Matches(bodyString)[0].Index;
                // remove key from key-value pair
                bodyString = KeyValueArrayItemExpression.Replace(bodyString, "{", 1);
                // remove curly braces around value
                bodyString = ReplaceCurlyBraces(bodyString, index, String.Empty, String.Empty);
                body = Encoding.UTF8.GetBytes(bodyString);
            }

            // For simplicity I assume that the outermost 'values' contains only 
            // one object.
            // TODO: This doesn't have to be the case.
            // I wonder what will happen if you e.g. get two contacts, and chain a create
            // to the request. Didn't try yet.
            // But if there is only one object, we just have to surround it's
            // curly braces by square braces.
            while (ValuesObjectInsteadOfArrayExpression.IsMatch(bodyString))
            {
                int index = ValuesObjectInsteadOfArrayExpression.Matches(bodyString)[0].Index + 9;
                bodyString = ValuesObjectInsteadOfArrayExpression.Replace(bodyString, "\"values\":{", 1);
                bodyString = ReplaceCurlyBraces(bodyString, index, "[{", "}]");
                body = Encoding.UTF8.GetBytes(bodyString);
            }
            return body;
        }

        private static string ReplaceCurlyBraces(string bodyString, int index, string newLeft, string newRight)
        {
            // Replace matching curly brace by square brace

            Debug.Assert(bodyString[index] == '{');
            int start = index + 1;

            var builder = new StringBuilder(bodyString.Substring(0, index));
            builder.Append(newLeft);

            int level = 0;
            while (level >= 0)
            {
                ++index;
                if (bodyString[index] == '{')
                {
                    ++level;
                }
                if (bodyString[index] == '}')
                {
                    --level;
                }
            }

            builder.Append(bodyString.Substring(start, index - start));
            builder.Append(newRight);
            builder.Append(bodyString.Substring(index + 1));
            return builder.ToString();
        }

        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            byte[] body;
            JsonSerializer serializer = new JsonSerializer();
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        writer.Formatting = Formatting.Indented;
                        if (parameters.Length == 1)
                        {
                            // Single parameter, assuming bare
                            serializer.Serialize(sw, parameters[0]);
                        }
                        else
                        {
                            writer.WriteStartObject();
                            for (int i = 0; i < this._operation.Messages[0].Body.Parts.Count; i++)
                            {
                                writer.WritePropertyName(this._operation.Messages[0].Body.Parts[i].Name);
                                serializer.Serialize(writer, parameters[0]);
                            }

                            writer.WriteEndObject();
                        }

                        writer.Flush();
                        sw.Flush();
                        body = ms.ToArray();
                    }
                }
            }

            Message requestMessage = Message.CreateMessage(messageVersion, _operation.Messages[0].Action, new RawBodyWriter(body));
            requestMessage.Headers.To = _operationUri;
            requestMessage.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Raw));
            HttpRequestMessageProperty reqProp = new HttpRequestMessageProperty();
            reqProp.Headers[HttpRequestHeader.ContentType] = "application/json";
            requestMessage.Properties.Add(HttpRequestMessageProperty.Name, reqProp);
            return requestMessage;
        }
    }
}
