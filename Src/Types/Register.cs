using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApi2026.Types
{
    public class Register
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "CPF é obrigatório")]
        public string Cpf { get; set; } = null!;

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Senha { get; set; } = null!;
    }
}
