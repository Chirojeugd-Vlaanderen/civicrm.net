// Thank you, Carlos Figueira
// http://blogs.msdn.com/b/endpoint/archive/2011/05/03/wcf-extensibility-message-formatters.aspx

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            using (MemoryStream ms = new MemoryStream(body))
            {
                using (var sr = new StreamReader(ms))
                {
                    Type returnType = this._operation.Messages[1].Body.ReturnValue.Type;
                    var result = serializer.Deserialize(sr, returnType);
                    return result;
                }
            }
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
