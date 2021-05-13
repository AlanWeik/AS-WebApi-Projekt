using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.APIKey
{
    public class Constants
    {
        public static readonly string AuthenticationSchemeName = "api key";
        public static readonly string HttpHeaderField = "X-Api-Key";
        public static readonly string HttpQueryParamKey = "ApiKey";

    }
}
