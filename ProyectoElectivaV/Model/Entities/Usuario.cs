using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoElectivaV.Model.Entities
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("nombre"), BsonRequired]
        public string nombre { get; set; }

        [BsonElement("apellido"), BsonRequired]
        public string apellido { get; set; }

        [BsonElement("email"), BsonRequired]
        public string email { get; set; }

        [BsonElement("contraseña"), BsonRequired]
        public string contraseña { get; set; }

        [BsonElement("Restablecer"), BsonRequired]
        public bool restablecer { get; set; }

        [BsonElement("Confirmado"), BsonRequired]
        public bool confirmado { get; set; }

        [BsonElement("Token"), BsonRequired]
        public string token { get; set; }
    }
}
