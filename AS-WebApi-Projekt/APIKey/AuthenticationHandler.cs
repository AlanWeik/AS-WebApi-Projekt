using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.APIKey
{
    public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AS_WebApi_ProjektContext _context;
        public AuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AS_WebApi_ProjektContext context
            )
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = Request.Headers[Constants.HttpHeaderField];
            if (token == null)
                token = Request.Query[Constants.HttpQueryParamKey];

            if (token == null)
                return AuthenticateResult.Fail("Invalid Authorization Header");

            List<ApiToken> ApiKeys = _context.ApiTokens.Include(a => a.User).ToList();
            foreach (var key in ApiKeys)
            {
                if (token == key.Value)
                {
                    var identity = new ClaimsIdentity(Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            return AuthenticateResult.Fail("FAIL.");
        }
    }
}
