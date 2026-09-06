using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2026.Types
{
    public class Login
    {
        public string? Nome { get; set; }

        [Required]
        public string User { get; set; } = null!;

        [Required]
        public string Senha { get; set; } = null!;
    }
}
