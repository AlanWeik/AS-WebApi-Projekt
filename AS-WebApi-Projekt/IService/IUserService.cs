using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_WebApi_Projekt.IService
{
    public interface IUserService
    {
        bool CheckUser(string username, string password);
    }
}
