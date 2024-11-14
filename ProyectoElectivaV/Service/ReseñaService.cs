using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using ProyectoElectivaV.DTOs.Reseñas;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.Excepciones;
using ProyectoElectivaV.Model.DbConfiguration;
using ProyectoElectivaV.Model.Entities;

namespace ProyectoElectivaV.Service
{
    public class ReseñaService
    {
        private readonly IMapper _mapeos;
        private readonly IMongoCollection<Serie> _serieCollection;

        public ReseñaService(IMapper mapeos,
                            IProyectoElectivaVDBSettings proyectoElectivaVDBSettings)
        {
            _mapeos = mapeos;
            var cliente = new MongoClient(proyectoElectivaVDBSettings.ConnectionString);
            var database = cliente.GetDatabase(proyectoElectivaVDBSettings.DatabaseName);
            _serieCollection = database.GetCollection<Serie>(proyectoElectivaVDBSettings.SeriesCollectionname);
        }

        public async Task<List<ReseñaDTO>> ObtenerTodasLasReseñas(string userEmail)
        {
            try
            {
                var series = await _serieCollection.Find(s => s.reseñas.Any(r => r.userEmail == userEmail)).ToListAsync();

                if (series == null || series.Count == 0)
                {
                    throw new CustomeExceptions("No hay reseñas disponibles para este usuario", 203);
                }

                List<ReseñaDTO> reseñasConSerie = new List<ReseñaDTO>();

                foreach (var serie in series)
                {
                    var reseñasFiltradas = serie.reseñas
                        .Where(r => r.userEmail == userEmail)
                        .Select(r => new ReseñaDTO
                        {
                            id = r.id.ToString(),
                            idSerie = r.idSerie,
                            userEmail = r.userEmail,
                            opinion = r.opinion,
                            clasificacion = r.clasificacion,
                            fechaCreacion = r.fechaCreacion,
                            cantidadMeDisgusta = r.cantidadMeDisgusta,
                            cantidadMeGusta = r.cantidadMeGusta,
                            Serie = _mapeos.Map<Serie, SerieDentroDeReseñaDTO>(serie)
                        }).ToList();

                    reseñasConSerie.AddRange(reseñasFiltradas);
                }

                return reseñasConSerie;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ReseñaDTO> CrearReseña(CrearReseñaDTO dto, string userEmail)
        {
            try
            {
                var filtroSerie = Builders<Serie>.Filter.Eq(s => s.id, dto.idSerie);
                var serie = await _serieCollection.Find(filtroSerie).FirstOrDefaultAsync();

                if (serie == null)
                {
                    throw new CustomeExceptions("La serie especificada no existe", 404);
                }

                var reseñaExistente = serie.reseñas.FirstOrDefault(r => r.userEmail == userEmail);
                if (reseñaExistente != null)
                {
                    throw new CustomeExceptions("El usuario ya ha realizado una reseña para esta serie", 403);
                }

                var nuevaReseña = _mapeos.Map<CrearReseñaDTO, Reseña>(dto);
                nuevaReseña.userEmail = userEmail;
                nuevaReseña.usuarioCreacion = userEmail;
                nuevaReseña.fechaCreacion = DateTime.Now;
                nuevaReseña.MeGustaEmails = new List<string>();
                nuevaReseña.MeDisgustaEmails = new List<string>();
                nuevaReseña.id = ObjectId.GenerateNewId();

                var actualizacion = Builders<Serie>.Update.Push(s => s.reseñas, nuevaReseña);
                await _serieCollection.UpdateOneAsync(filtroSerie, actualizacion);

                await ActualizarPuntuacionDeSerie(dto.idSerie);

                var reseñaDTO = _mapeos.Map<Reseña, ReseñaDTO>(nuevaReseña);

                return reseñaDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReseñaDTO> ObtenerReseña(string idSerie, string userEmail)
        {
            try
            {
                var filtroSerie = Builders<Serie>.Filter.Eq(s => s.id, idSerie);
                var serie = await _serieCollection.Find(filtroSerie).FirstOrDefaultAsync();

                if (serie == null)
                {
                    throw new CustomeExceptions("La serie no fue encontrada", 404);
                }

                var reseña = serie.reseñas.FirstOrDefault(r => r.userEmail == userEmail);

                if (reseña == null)
                {
                    throw new CustomeExceptions("La reseña no fue encontrada", 404);
                }

                var reseñaDTO = _mapeos.Map<Reseña, ReseñaDTO>(reseña);

                return reseñaDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReseñaDTO> ActualizarReseña(string userEmail, string idReseña, CrearReseñaDTO dto)
        {
            try
            {
                ObjectId objectIdReseña = ObjectId.Parse(idReseña);

                var filtroSerie = Builders<Serie>.Filter.Eq(s => s.id, dto.idSerie);
                var serie = await _serieCollection.Find(filtroSerie).FirstOrDefaultAsync();

                if (serie == null)
                {
                    throw new CustomeExceptions("La serie no fue encontrada", 404);
                }

                var reseña = serie.reseñas.FirstOrDefault(r => r.id == objectIdReseña);

                if (reseña == null)
                {
                    throw new CustomeExceptions("La reseña no fue encontrada", 404);
                }

                if (reseña.userEmail != userEmail)
                {
                    throw new CustomeExceptions("No tienes permiso para actualizar esta reseña", 403);
                }

                reseña.opinion = dto.opinion ?? reseña.opinion;
                reseña.clasificacion = dto.clasificacion != 0 ? dto.clasificacion : reseña.clasificacion;
                reseña.usuarioActualizacion = userEmail;
                reseña.fechaActualizacion = DateTime.Now;

                var filtroReseña = Builders<Serie>.Filter.ElemMatch(s => s.reseñas, r => r.id == objectIdReseña);

                var actualizacion = Builders<Serie>.Update
                    .Set("reseñas.$.opinion", reseña.opinion)
                    .Set("reseñas.$.clasificacion", reseña.clasificacion)
                    .Set("reseñas.$.fechaActualizacion", reseña.fechaActualizacion);

                await _serieCollection.UpdateOneAsync(filtroSerie & filtroReseña, actualizacion);

                await ActualizarPuntuacionDeSerie(dto.idSerie);

                var reseñaDTO = _mapeos.Map<Reseña, ReseñaDTO>(reseña);

                return reseñaDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ReseñaDTO> CalificarReseña(string userEmail, string idReseña, CalificarReseñaDTO dto)
        {
            try
            {
                ObjectId objectIdReseña = ObjectId.Parse(idReseña);

                var filtroSerie = Builders<Serie>.Filter.Eq(s => s.id, dto.idSerie);
                var serie = await _serieCollection.Find(filtroSerie).FirstOrDefaultAsync();

                if (serie == null)
                {
                    throw new CustomeExceptions("La serie no fue encontrada", 404);
                }

                var reseña = serie.reseñas.FirstOrDefault(r => r.id == objectIdReseña);
                if (reseña == null)
                {
                    throw new CustomeExceptions("La reseña no fue encontrada", 404);
                }

                if (dto.meGusta == true)
                {
                    if (!reseña.MeGustaEmails.Contains(userEmail))
                    {
                        reseña.MeGustaEmails.Add(userEmail);
                        reseña.cantidadMeGusta = reseña.MeGustaEmails.Count;

                        if (reseña.MeDisgustaEmails.Remove(userEmail))
                        {
                            reseña.cantidadMeDisgusta = reseña.MeDisgustaEmails.Count;
                        }
                    }
                }
                else if (dto.meDisgusta == true)
                {
                    if (!reseña.MeDisgustaEmails.Contains(userEmail))
                    {
                        reseña.MeDisgustaEmails.Add(userEmail);
                        reseña.cantidadMeDisgusta = reseña.MeDisgustaEmails.Count;

                        if (reseña.MeGustaEmails.Remove(userEmail))
                        {
                            reseña.cantidadMeGusta = reseña.MeGustaEmails.Count;
                        }
                    }
                }

                reseña.usuarioActualizacion = userEmail;
                reseña.fechaActualizacion = DateTime.Now;

                var filtroReseña = Builders<Serie>.Filter.ElemMatch(s => s.reseñas, r => r.id == objectIdReseña);
                var actualizacion = Builders<Serie>.Update
                    .Set("reseñas.$.MeGustaEmails", reseña.MeGustaEmails)
                    .Set("reseñas.$.MeDisgustaEmails", reseña.MeDisgustaEmails)
                    .Set("reseñas.$.cantidadMeGusta", reseña.cantidadMeGusta)
                    .Set("reseñas.$.cantidadMeDisgusta", reseña.cantidadMeDisgusta)
                    .Set("reseñas.$.fechaActualizacion", reseña.fechaActualizacion);

                await _serieCollection.UpdateOneAsync(filtroSerie & filtroReseña, actualizacion);

                var reseñaDTO = _mapeos.Map<Reseña, ReseñaDTO>(reseña);

                return reseñaDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> EliminarReseña(string userEmail, string idReseña)
        {
            try
            {
                ObjectId objectIdReseña = ObjectId.Parse(idReseña);
                var filtroSerie = Builders<Serie>.Filter.ElemMatch(s => s.reseñas, r => r.id == objectIdReseña);
                var serie = await _serieCollection.Find(filtroSerie).FirstOrDefaultAsync();

                if (serie == null)
                {
                    throw new CustomeExceptions("La serie no fue encontrada", 404);
                }

                var reseña = serie.reseñas.FirstOrDefault(r => r.id == objectIdReseña);

                if (reseña == null)
                {
                    throw new CustomeExceptions("La reseña no fue encontrada", 404);
                }

                if (reseña.userEmail != userEmail)
                {
                    throw new CustomeExceptions("No tienes permiso para eliminar esta reseña", 403);
                }

                var filtroReseña = Builders<Serie>.Filter.ElemMatch(s => s.reseñas, r => r.id == objectIdReseña);
                var actualizacion = Builders<Serie>.Update.PullFilter(s => s.reseñas, r => r.id == objectIdReseña);

                var resultado = await _serieCollection.UpdateOneAsync(filtroSerie & filtroReseña, actualizacion);

                if (resultado.ModifiedCount == 0)
                {
                    throw new CustomeExceptions("No se pudo eliminar la reseña", 500);
                }

                return "Reseña eliminada con exito";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ActualizarPuntuacionDeSerie(string idSerie)
        {
            var filtro = Builders<Serie>.Filter.Eq(s => s.id, idSerie);
            var serie = await _serieCollection.Find(filtro).FirstOrDefaultAsync();

            if (serie != null && serie.reseñas.Any())
            {
                double promedioClasificacion = serie.reseñas.Average(r => r.clasificacion);

                var actualizacion = Builders<Serie>.Update.Set(s => s.puntuacion, promedioClasificacion);

                await _serieCollection.UpdateOneAsync(filtro, actualizacion);
            }
            else
            {
                Console.WriteLine("No se encontraron reseñas para esta serie.");
            }
        }


    }

}
