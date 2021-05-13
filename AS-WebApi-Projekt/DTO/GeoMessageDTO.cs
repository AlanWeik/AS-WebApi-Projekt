using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.DTO
{
    public class V1GetDTO
    {
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class V2MessageDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
    }

    public class V2GetDTO
    {
        public V2MessageDTO Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class V2MessagePostDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class V2PostDTO
    {
        public V2MessagePostDTO Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
