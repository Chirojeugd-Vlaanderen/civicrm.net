using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chiro.CiviCrm.Api.DataContracts
{
    /// <summary>
    /// Some class that basically converts an ID to the json-part of the request url.
    /// </summary>
    public class CiviId
    {
        public int Id { get; set; }

        public CiviId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return String.Format("{{'sequential':1, id:{0}}}", Id);
        }
    }
}
