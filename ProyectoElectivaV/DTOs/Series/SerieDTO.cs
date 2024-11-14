using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProyectoElectivaV.Model.Entities;

namespace ProyectoElectivaV.DTOs.Series
{
    public class SerieDTO
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string plataforma { get; set; }
        public List<string> categorias { get; set; }
        public int episodios { get; set; }
        public DateTime fechaDeEstreno { get; set; }
        public string sinopsis { get; set; }
        public int visitas { get; set; }
        public List<Reseña> reseñas { get; set; }
    }
}
