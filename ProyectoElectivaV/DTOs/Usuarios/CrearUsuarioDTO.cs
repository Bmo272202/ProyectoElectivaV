namespace ProyectoElectivaV.DTOs.Usuarios
{
    public class CrearUsuarioDTO
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public string confirmarContraseña { get; set; }
        public string token { get; set; }
    }
}
