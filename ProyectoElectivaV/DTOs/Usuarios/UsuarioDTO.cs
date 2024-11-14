using MongoDB.Bson.Serialization.Attributes;

namespace ProyectoElectivaV.DTOs.Usuarios
{
    public class UsuarioDTO
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public bool restablecer { get; set; }
        public bool confirmado { get; set; }
        public string token { get; set; }
    }
}
