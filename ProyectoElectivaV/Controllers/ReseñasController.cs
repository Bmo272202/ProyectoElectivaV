using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProyectoElectivaV.DTOs.Reseñas;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Service;
using System.Security.Claims;

namespace ProyectoElectivaV.Controllers
{
    [Route("reseñasController")]
    [Authorize]
    [ApiController]
    public class ReseñasController : ControllerBase
    {
        private readonly ReseniaService _reseñaService;

        public ReseñasController(ReseniaService reseñaService)
        {
            _reseñaService = reseñaService;
        }

        [HttpGet("ObtenerTodasLasReseñas")]
        public async Task<IActionResult> ObtenerTodasLasReseñas()
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.ObtenerTodasLasReseñas(userEmail);
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

        [HttpPost("CrearReseña")]
        public async Task<IActionResult> CrearReseña([FromBody] CrearReseñaDTO dto)
        {
            try
            {
                
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.CrearReseña(dto, userEmail);
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

        [HttpGet("ObtenerReseña/{idSerie}")]
        public async Task<IActionResult> ObtenerReseña(string idSerie)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.ObtenerReseña(idSerie, userEmail);
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

        [HttpPut("ActualizarReseña/{idReseña}")]
        public async Task<IActionResult> ActualizarReseña(string idReseña, [FromBody] CrearReseñaDTO dto)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.ActualizarReseña(userEmail, idReseña, dto);
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

        [HttpPut("CalificarReseña/{idReseña}")]
        public async Task<IActionResult> CalificarReseña(string idReseña, [FromBody] CalificarReseñaDTO dto)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.CalificarReseña(userEmail, idReseña, dto);
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

        [HttpDelete("EliminarReseña/{idReseña}")]
        public async Task<IActionResult> EliminarReseña(string idReseña)
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("No se pudo obtener el email del usuario autenticado.");
                }

                var result = await _reseñaService.EliminarReseña(userEmail, idReseña);
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
    }
}
