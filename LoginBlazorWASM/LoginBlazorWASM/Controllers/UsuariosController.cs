using LoginBlazorWASM.Data;
using LoginBlazorWASM.Shared.src;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LoginBlazorWASM.Services;


namespace LoginBlazorWASM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        private AuthService _authService;

        public UsuariosController(DataContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Usuario>>> GetAllUsers()
        {
            return await _context.Usuarios.ToListAsync();
        }
        
        [HttpGet("GetDatos")]
        public async Task<ActionResult<List<Usuario>>> GetCuenta()
        {
            var lista = await _context.Usuarios.ToListAsync();
            return Ok(lista);
        }

        [HttpPut("Cambiar")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> CambiarContrasenia(UsuarioDTO user)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(user.user_name) || string.IsNullOrEmpty(user.password) || string.IsNullOrEmpty(user.new_password))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            // Buscar el usuario en la base de datos
            var cuenta = await _context.Usuarios
                .Where(x => x.UserName == user.user_name)
                .FirstOrDefaultAsync();

            if (cuenta == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Verificar que la contraseña actual coincida
            if (_authService.VerifyPasswordHash(user.password,cuenta.PasswordHash,cuenta.PasswordSalt))
            {
                return BadRequest("La contraseña actual no coincide.");
            }

            // Actualizar la contraseña
            _authService.CreatePasswordHash(user.new_password, out byte[] passwordHash, out byte[] passwordSalt);
            cuenta.PasswordHash = passwordHash;
            cuenta.PasswordSalt= passwordSalt;

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Se cambio la contraseña correctamente");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task <ActionResult<string>> IniciarSesion(UsuarioDTO obj)
        {
            var cuenta = await _context.Usuarios.Where(x => x.UserName == obj.user_name).FirstOrDefaultAsync();
            bool valido = _authService.VerifyPasswordHash(obj.password, cuenta.PasswordHash, cuenta.PasswordSalt);

            if (cuenta == null || !valido)
            {
                return BadRequest("Usuario y/o contraseña incorrectos");
            }

            string token = _authService.CreateToken(cuenta);
            return Ok(token);
        }

        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task <ActionResult<string>> CreateCuenta(UsuarioDTO user)
        {
           
            var cuenta = await _context.Usuarios.Where(x => x.UserName == user.user_name).FirstOrDefaultAsync();
            if (cuenta != null)
            {
                return BadRequest("credenciales ya ocupadas");
            }
            else
            {
                var usuario = new Usuario();

                _authService.CreatePasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.UserName = user.user_name;
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
                usuario.Rol = "Usuario";
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok("Registrado con exito");
            }
        }
    }
}

