using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.DTOs.Usuarios;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Model.Entities;
using ProyectoElectivaV.Service;
using System.Security.Claims;

namespace ProyectoElectivaV.Controllers
{
    [Route("usuarioController")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] CrearUsuarioDTO dto)
        {
            try
            {
                var result = await _usuarioService.RegistrarUsuario(dto);
                return Ok(result);

            }
            catch (CustomeExceptions ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario([FromBody] LoginDTO dto)
        {
            try
            {
                var result = await _usuarioService.LoginUsuario(dto);
                return Ok(result);

            }
            catch (CustomeExceptions ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ObtenerUsuario")]
        [Authorize]
        public async Task<IActionResult> ObtenerUsuario()
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _usuarioService.ObtenerUsuario(userEmail);
                return Ok(result);

            }
            catch (CustomeExceptions ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("RestablecerContraseña")]
        [Authorize]
        public async Task<IActionResult> RestablecerContraseña([FromBody] RestablecerContraseñaDTO dto)
        {
            try
            {
                var result = await _usuarioService.RestablecerContraseña(dto);
                return Ok(result);
            }
            catch (CustomeExceptions ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

    }
}
