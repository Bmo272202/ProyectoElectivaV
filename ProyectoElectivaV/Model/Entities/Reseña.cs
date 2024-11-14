using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProyectoElectivaV.Model.Entities
{
    public class Reseña
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("ID_serie"), BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string idSerie { get; set; }

        [BsonElement("ID_usuario"), BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string idUsuario { get; set; }

        [BsonElement("Fecha_Creacion"), BsonRequired]
        public DateTime fechaCreacion { get; set; }

        [BsonElement("Fecha_Actualizacion"), BsonRequired]
        public DateTime fechaActualizacion { get; set; }

        [BsonElement("Opinion"), BsonRequired]
        public string opinion { get; set; }

        [BsonElement("Clasificacion"), BsonRequired]
        public double clasificacion { get; set; }

        [BsonElement("Me_gusta")]
        public bool? meGusta { get; set; } = null;

        [BsonElement("Me_disgusta")]
        public bool? meDisgusta { get; set; } = null;

        [BsonElement("Cantidad_dislikes")]
        public int? cantidadMeGusta { get; set; }

        [BsonElement("Cantidad_likes")]
        public int? cantidadMeDisgusta { get; set; }

        [BsonElement("Usuario_actualizacion"), BsonRequired]
        public string usuarioActualizacion { get; set; }

        [BsonElement("Usuario_creacion"), BsonRequired]
        public string usuarioCreacion { get; set; }
    }
}
