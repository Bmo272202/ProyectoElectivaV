using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Model.DbConfiguration;
using ProyectoElectivaV.Model.Entities;

namespace ProyectoElectivaV.Service
{
    public class SerieService
    {
        private readonly IMapper _mapeos;
        private readonly IMongoCollection<Serie> _serieCollection;

        public SerieService(IMapper mapeos,
                            IProyectoElectivaVDBSettings proyectoElectivaVDBSettings) 
        {
            _mapeos = mapeos;
            var cliente = new MongoClient(proyectoElectivaVDBSettings.ConnectionString);
            var database = cliente.GetDatabase(proyectoElectivaVDBSettings.DatabaseName);
            _serieCollection = database.GetCollection<Serie>(proyectoElectivaVDBSettings.SeriesCollectionname);
        }

        public async Task<List<SerieDTO>> ObtenerTodasLasSeries()
        {
            try
            {
                List<Serie> series = await _serieCollection.Find(s => true).ToListAsync();

                if (series is null)
                {
                    throw new CustomeExceptions("No hay series disponibles", 203);
                }

                var result = _mapeos.Map<List<Serie>,List<SerieDTO>>(series);
                
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SerieDTO> CrearSerie(CrearSerieDTO dto)
        {
            try
            {
                var serie = _mapeos.Map<CrearSerieDTO, Serie>(dto);
                serie.reseñas = new List<Reseña>();
                await _serieCollection.InsertOneAsync(serie);

                return _mapeos.Map<SerieDTO>(serie);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SerieDTO> ObtenerSerie(string idSerie)
        {
            try
            {
                Serie exist = await _serieCollection.Find(s => s.id == idSerie).FirstOrDefaultAsync();

                if (exist is null)
                {
                    throw new CustomeExceptions("La serie solicitada no existe", 203);
                }

                exist.visitas += 1;

                var filter = Builders<Serie>.Filter.Eq(s => s.id, idSerie);
                var update = Builders<Serie>.Update.Set(s => s.visitas, exist.visitas);

                await _serieCollection.UpdateOneAsync(filter, update);

                var serie = _mapeos.Map<Serie, SerieDTO>(exist);

                return serie;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CrearSerieDTO> ActualizarSerie(string idSerie, CrearSerieDTO dto)
        {
            try
            {
                Serie exist = await _serieCollection.Find(s => s.id == idSerie).FirstOrDefaultAsync();

                if (exist is null)
                {
                    throw new CustomeExceptions("La serie solicitada no existe", 203);
                }

                var serie = _mapeos.Map<CrearSerieDTO, Serie>(dto);

                await _serieCollection.ReplaceOneAsync(s => s.id == idSerie, serie);

                return _mapeos.Map<CrearSerieDTO>(serie);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<object> EliminarSerie(string idSerie)
        {
            try
            {
                Serie exist = await _serieCollection.Find(s => s.id == idSerie).FirstOrDefaultAsync();

                if (exist is null)
                {
                    throw new CustomeExceptions("La serie solicitada no existe", 203);
                }

                await _serieCollection.DeleteOneAsync(s => s.id == idSerie);

                return "Registro eliminado con exito";
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SerieDTO>> ObtenerSeriesPorNombre(string nombre)
        {
            try
            {
                var filter = Builders<Serie>.Filter.Regex(s => s.nombre, new BsonRegularExpression(nombre, "i")); // Búsqueda insensible a mayúsculas

                List<Serie> series = await _serieCollection.Find(filter).ToListAsync();

                if (series is null || series.Count == 0)
                {
                    throw new CustomeExceptions("No se encontraron series con ese nombre", 203);
                }

                var result = _mapeos.Map<List<Serie>, List<SerieDTO>>(series);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<SerieDTO>> ObtenerSeriesPorCategorias(List<string> categorias)
        {
            try
            {
                var filter = Builders<Serie>.Filter.Or(
                    categorias.Select(categoria => Builders<Serie>.Filter.Regex(s => s.categorias, new BsonRegularExpression(categoria, "i"))).ToArray()
                );

                List<Serie> series = await _serieCollection.Find(filter).ToListAsync();

                if (series == null || series.Count == 0)
                {
                    throw new CustomeExceptions("No se encontraron series para las categorías proporcionadas", 203);
                }

                var result = _mapeos.Map<List<Serie>, List<SerieDTO>>(series);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las series por categorías: " + ex.Message);
            }
        }


    }
}
