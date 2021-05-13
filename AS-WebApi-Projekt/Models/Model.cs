using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.Models
{
    public class GeoMessageV1
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class GeoMessageV2 : GeoMessageV1
    {
        public new Message Message { get; set; }
    }
    public class Message
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
    }
    public class ApiToken
    {
        public int ID { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
