using Microsoft.AspNetCore.Mvc;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Service;

namespace ProyectoElectivaV.Controllers
{
    [Route("seriesController")]
    [ApiController]
    public class SerieController : ControllerBase
    {
        private readonly SerieService _serieService;

        public SerieController(SerieService serieService)
        {
            _serieService = serieService;
        }

        [HttpGet("ObtenerTodasLasSeries")]
        public async Task<IActionResult> ObtenerTodasLasSeries() {
            try
            {
                var result = await _serieService.ObtenerTodasLasSeries();
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

        [HttpPost("CrearSerie")]
        public async Task<IActionResult> CrearSerie([FromBody] CrearSerieDTO dto)
        {
            try
            {
                var result = await _serieService.CrearSerie(dto);
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

        [HttpGet("ObtenerSerie/{idSerie}")]
        public async Task<IActionResult> ObtenerSerie(string idSerie)
        {
            try
            {
                var result = await _serieService.ObtenerSerie(idSerie);
                return Ok(result);

            }
            catch(CustomeExceptions ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("ActualizarSerie/{idSerie}")]
        public async Task<IActionResult> ActualizarSerie(string idSerie, [FromBody] CrearSerieDTO dto)
        {
            try
            {
                var result = await _serieService.ActualizarSerie(idSerie, dto);
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

        [HttpDelete("EliminarSerie/{idSerie}")]
        public async Task<IActionResult> EliminarSerie(string idSerie)
        {
            try
            {
                var result = await _serieService.EliminarSerie(idSerie);
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

        [HttpGet("ObtenerSeriesPorNombre/{nombre}")]
        public async Task<IActionResult> ObtenerSeriesPorNombre(string nombre)
        {
            try
            {
                var result = await _serieService.ObtenerSeriesPorNombre(nombre);
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

        [HttpGet("ObtenerSeriesPorCategorias")]
        public async Task<IActionResult> ObtenerSeriesPorCategorias([FromQuery] List<string> categorias)
        {
            try
            {
                var result = await _serieService.ObtenerSeriesPorCategorias(categorias);
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
