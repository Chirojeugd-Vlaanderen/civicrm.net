using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chiro.CiviCrm.BehaviorExtension
{
    /// <summary>
    /// Use this attribute to mark datacontracts that are to be converted to
    /// Json by the JsonQueryStringConverter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonConvertibleAttribute: System.Attribute
    {
    }
}
