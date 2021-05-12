using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.Models
{
    public class ApiToken
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
}