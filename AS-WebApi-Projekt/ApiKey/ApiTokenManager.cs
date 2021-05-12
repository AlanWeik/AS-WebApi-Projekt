using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AS_WebApi_Projekt.Data;

namespace AS_WebApi_Projekt.ApiKey
{
    public class ApiTokenManager
    {


        private readonly AS_WebApi_ProjektContext _context;
        public ApiTokenManager(AS_WebApi_ProjektContext context)
        {
            _context = context;
        }

    }
}
