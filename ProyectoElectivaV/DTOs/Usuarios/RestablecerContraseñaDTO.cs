namespace ProyectoElectivaV.DTOs.Usuarios
{
    public class RestablecerContraseñaDTO
    {
        public string contraseña { get; set; }
        public string confirmarContraseña { get; set; }
        public string token { get; set; }
    }
}
