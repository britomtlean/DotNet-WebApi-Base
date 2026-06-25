using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi2026.Types; //TYPE
using WebApi2026.Entities; //ENTITIE

namespace WebApi2026.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(Login login);

        Task<Object> Register(Usuario register);
    }
}
