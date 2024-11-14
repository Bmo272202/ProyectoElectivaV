using AutoMapper;
using ProyectoElectivaV.DTOs.Series;
using ProyectoElectivaV.DTOs.Usuarios;
using ProyectoElectivaV.Model.Entities;

namespace ProyectoElectivaV.Mapeos
{
    public class ProyectoMap : Profile
    {
        public ProyectoMap() {
            
            CreateMap<Serie, SerieDTO>().ReverseMap();
            CreateMap<Serie, CrearSerieDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Usuario, CrearUsuarioDTO>().ReverseMap();
        }
    }
}
