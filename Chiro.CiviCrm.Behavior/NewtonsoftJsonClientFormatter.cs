/*
   Based on code of Carlus Figueira
   http://blogs.msdn.com/b/endpoint/archive/2011/05/03/wcf-extensibility-message-formatters.aspx
  
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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Chiro.CiviCrm.BehaviorExtension
{
    class NewtonsoftJsonClientFormatter : IClientMessageFormatter
    {
        readonly OperationDescription _operation;
        readonly Uri _operationUri;

        private static readonly Regex ValuesObjectInsteadOfArrayExpression = new Regex("\"values\":[{]");
        private static readonly Regex KeyValueArrayItemExpression = new Regex("\"[0-9]+\":[{]");

        public NewtonsoftJsonClientFormatter(OperationDescription operation, ServiceEndpoint endpoint)
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

            body = ChainingArrayResultWorkAround(body);

            using (var ms = new MemoryStream(body))
            {
                using (var sr = new StreamReader(ms))
                {
                    Type returnType = this._operation.Messages[1].Body.ReturnValue.Type;
                    var result = serializer.Deserialize(sr, returnType);
                    return result;
                }
            }
        }

        private static byte[] ChainingArrayResultWorkAround(byte[] body)
        {
            // If you chain an 'api.create' with multiple entities of the same type, the resulting Json cannot
            // be parsed by the serializer. See this thread on the civicrm forums:
            // http://forum.civicrm.org/index.php/topic,35393.0.html

            // TODO: can't we move this to a converter?

            string bodyString = Encoding.UTF8.GetString(body);

            // Work around this issue:
            while (KeyValueArrayItemExpression.IsMatch(bodyString))
            {
                int index = KeyValueArrayItemExpression.Matches(bodyString)[0].Index;
                bodyString = KeyValueArrayItemExpression.Replace(bodyString, "{", 1);
                bodyString = ReplaceCurlyBraces(bodyString, index, String.Empty, String.Empty);
                body = Encoding.UTF8.GetBytes(bodyString);
            }
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
