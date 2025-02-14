using AuthRoleApp.Shared;
using LoginBlazorWASM.Data;
using LoginBlazorWASM.Shared.src;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginBlazorWASM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;

        public UsuariosController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAllUsers()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /*[HttpPut ("CambiarPassword")]
        public async Task <ActionResult<Usuario>> CambiarContrasenia(UsuarioDTO user)
        {
            var cuenta = await _context.Usuarios.Where(x => x.UserName == user.user_name).FirstOrDefaultAsync();
            bool valido = false;
            if (cuenta != null)
            {
                if(cuenta.Password == user.password) 
                {
                    valido = true;
                    cuenta.Password = user.new_password;
                }
            }
            else
            {
                return BadRequest("Usuario no encontrado");
            }
               
            if (!valido)
            {
                return BadRequest("La vieja contraseña ingresada no coincide");
            }
            else
            {
                return Ok(cuenta);
            }

            
        }*/
        [HttpPut("Cambiar")]
        public async Task<ActionResult<Usuario>> CambiarContrasenia(UsuarioDTO user)
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
            if (cuenta.Password != user.password)
            {
                return BadRequest("La contraseña actual no coincide.");
            }

            // Actualizar la contraseña
            cuenta.Password = user.new_password;

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(cuenta);
        }

        [HttpPost("Login")]
        public async Task <ActionResult<bool>> IniciarSesion(UsuarioDTO user)
        {
            var cuenta = await _context.Usuarios.Where(x => x.UserName == user.user_name).FirstOrDefaultAsync();
            bool valido = false;

            if (cuenta != null) 
                valido = (user.password == cuenta.Password);
                
            if (cuenta == null || !valido)
            {
                return BadRequest("Usuario y/o contraseña incorrectos");
            }

            return Ok(valido);
        }

        [HttpPost("Registrar")]
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
                usuario.UserName = user.user_name;
                usuario.Password = user.password;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return Ok("Registrado con exito");
            }
        }
    }
}
