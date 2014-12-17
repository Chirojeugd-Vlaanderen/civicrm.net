// Thank you, Carlos Figueira
// http://blogs.msdn.com/b/endpoint/archive/2011/05/03/wcf-extensibility-message-formatters.aspx

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;

namespace Chiro.CiviCrm.BehaviorExtension
{
    public class MyRawMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }
    }
}
