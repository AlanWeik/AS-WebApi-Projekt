using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;

namespace AS_WebApi_Projekt.ApiKey
{
    public class ApiTokenManager
    {


        private readonly AS_WebApi_ProjektContext _context;
        public ApiTokenManager(AS_WebApi_ProjektContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            // Finns det redan token?
            var token = await _context.ApiTokens
                .FirstOrDefaultAsync(t => t.User.Id == user.Id);

            token ??= new ApiToken();

            token.Value = Guid.NewGuid().ToString();
            token.User = user;

            _context.ApiTokens.Update(token);
            await _context.SaveChangesAsync();

            return token.Value;
        }
    }
}
