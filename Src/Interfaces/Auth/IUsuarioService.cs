using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using WebApi2026.Entities;

namespace WebApi2026.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> GetUnique(string id);
        // Busca um usuario pelo Id
    }
}
