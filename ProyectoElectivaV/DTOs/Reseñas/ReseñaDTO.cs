using MongoDB.Bson.Serialization.Attributes;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.Model.Entities;

namespace ProyectoElectivaV.DTOs.Reseñas
{
    public class ReseñaDTO
    {
        public string id { get; set; }
        public string idSerie { get; set; }
        public string userEmail { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public string opinion { get; set; }
        public double clasificacion { get; set; }
        public int? cantidadMeGusta { get; set; }
        public int? cantidadMeDisgusta { get; set; }
        public string usuarioActualizacion { get; set; }
        public string usuarioCreacion { get; set; }
        public SerieDentroDeReseñaDTO Serie { get; set; }
        public List<string> MeGustaEmails { get; set; } = new List<string>();
        public List<string> MeDisgustaEmails { get; set; } = new List<string>();
    }
}
