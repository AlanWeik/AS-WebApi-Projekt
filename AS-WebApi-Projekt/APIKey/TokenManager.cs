using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.APIKey
{
    public class TokenManager
    {
        private readonly AS_WebApi_ProjektContext _context;
        public TokenManager(AS_WebApi_ProjektContext context)
        {
            _context = context; 
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var token = await _context.ApiTokens.FirstOrDefaultAsync(t => t.User.Id == user.Id);
            token ??= new ApiToken();
            token.Value = Guid.NewGuid().ToString();
            token.User = user;
            _context.ApiTokens.Update(token);
            _context.SaveChanges();
            return token.Value;
        }
    }
}
