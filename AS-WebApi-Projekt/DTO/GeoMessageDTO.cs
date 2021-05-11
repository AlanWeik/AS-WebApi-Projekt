using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.DTO
{
    public class V2MessageDTO
    {
        public string title { get; set; }
        public string body { get; set; }
        public string author { get; set; }
    }

    public class V2GetDTO
    {
        public V2MessageDTO message { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
    public class V2MessagePostDTO
    {
        public string title { get; set; }
        public string body { get; set; }
    }

    public class V2PostDTO
    {
        public V2MessagePostDTO message { get; set; }
        public double longitude { get; set; }
        public double latityde { get; set; }
    }
}
