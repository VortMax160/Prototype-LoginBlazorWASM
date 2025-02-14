using System.ComponentModel.DataAnnotations;

namespace AuthRoleApp.Shared
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string? user_name { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? password { get; set; }

        public string new_password { get; set; } = String.Empty;
    }
}
