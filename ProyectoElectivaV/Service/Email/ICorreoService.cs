using ProyectoElectivaV.DTOs.Usuarios;

namespace ProyectoElectivaV.Service.Email
{
    public interface ICorreoService
    {
        void EnviarEmail(CorreoDTO correo);
    }
}
