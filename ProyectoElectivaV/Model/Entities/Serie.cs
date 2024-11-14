
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoElectivaV.Model.Entities
{
    public class Serie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("Nombre"), BsonRequired]
        public string nombre { get; set; }

        [BsonElement("Plataforma"), BsonRequired]
        public string plataforma { get; set; }

        [BsonElement("Categorías"), BsonRequired]
        public List<string> categorias { get; set; }

        [BsonElement("Episodios"), BsonRequired]
        public int episodios { get; set; }

        [BsonElement("Fecha_de_estreno"), BsonRequired]
        public DateTime fechaDeEstreno { get; set; }

        [BsonElement("Sinopsis"), BsonRequired]
        public string sinopsis { get; set; }

        [BsonElement("visitas"), BsonRequired]
        public int visitas { get; set; }

        public List<Reseña> reseñas { get; set; }
    }
}
