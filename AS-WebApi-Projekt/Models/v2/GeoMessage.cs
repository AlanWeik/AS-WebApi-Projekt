using AS_WebApi_Projekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.Models.v2
{
    public class GeoMessageV2
    {
        public int ID { get; set; }
        public string message { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }

    }
}
