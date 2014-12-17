// Thank you, Carlos Figueira
// http://blogs.msdn.com/b/endpoint/archive/2011/05/03/wcf-extensibility-message-formatters.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Chiro.CiviCrm.BehaviorExtension
{
    class RawBodyWriter : BodyWriter
    {
        byte[] content;
        public RawBodyWriter(byte[] content)
            : base(true)
        {
            this.content = content;
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("Binary");
            writer.WriteBase64(content, 0, content.Length);
            writer.WriteEndElement();
        }
    }

}
