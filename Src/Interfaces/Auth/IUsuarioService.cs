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
        Task Create(Usuario usuario);
        //Cria usuario

        Task<List<Usuario>> GetAll();
        //Envia todos os usuarios

        Task<Usuario?> GetUnique(string id);
        // Busca um usuario pelo Id

        Task<Object> Delete(string id);
        // Deleta um usuario pelo Id

        Task UpdateNome(string id, string nome);
        // Atualiza apenas o nome do usuario
    }
}
