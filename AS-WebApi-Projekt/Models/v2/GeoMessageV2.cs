using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.Models.v2
{
    public class GeoMessageV2 : GeoMessageV1
    {
        public new Message message { get; set; }
    }
}
