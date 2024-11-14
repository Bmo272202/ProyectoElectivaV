using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProyectoElectivaV.DTOs.Reseñas
{
    public class CrearReseñaDTO
    {
        public string idSerie { get; set; }
        public string opinion { get; set; }
        public double clasificacion { get; set; }
    }
}
