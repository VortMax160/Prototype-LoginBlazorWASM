using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginBlazorWASM.Shared.src
{
    public class Usuario
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string? Token { get; set; }

        public string? Rol { get; set; }
    }
}
