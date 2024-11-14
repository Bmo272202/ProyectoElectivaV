using Microsoft.AspNetCore.Mvc;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.DTOs.Usuarios;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Model.Entities;
using ProyectoElectivaV.Service;

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

        //[HttpGet("ObtenerTodosLosUsuarios")]
        //public async Task<IActionResult> ObtenerTodosLosUsuarios()
        //{
        //    try
        //    {
        //        var result = await _usuarioService.ObtenerTodosLosUsuarios();
        //        return Ok(result);
        //    }
        //    catch (CustomeExceptions ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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

        //[HttpPut("ConfirmarCorreo/{token}")]
        //public async Task<IActionResult> ConfirmarCorreo(string token)
        //{
        //    try
        //    {
        //        var result = await _usuarioService.ConfirmarCorreo(token);
        //        return Ok(result);

        //    }
        //    catch (CustomeExceptions ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

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

        //[HttpGet("ObtenerUsuario/{email}")]
        //public async Task<IActionResult> ObtenerUsuario(string email)
        //{
        //    try
        //    {
        //        var result = await _usuarioService.ObtenerUsuario(email);
        //        return Ok(result);

        //    }
        //    catch (CustomeExceptions ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpPut("RestablecerContraseña")]
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
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpDelete("EliminarUsuario/{idUsuario}")]
        //public async Task<IActionResult> EliminarUsuario(string idUsuario)
        //{
        //    try
        //    {
        //        var result = await _usuarioService.EliminarUsuario(idUsuario);
        //        return Ok(result);

        //    }
        //    catch (CustomeExceptions ex)
        //    {
        //        return StatusCode(ex.StatusCode, ex.Message);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        

    }
}
