using System.ComponentModel.DataAnnotations;

namespace LoginBlazorWASM.Shared.src
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string user_name { get; set; } = String.Empty;
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string password { get; set; } = String.Empty;

        public string new_password { get; set; } = String.Empty;
    }
}
