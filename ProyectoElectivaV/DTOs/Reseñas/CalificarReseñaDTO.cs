namespace ProyectoElectivaV.DTOs.Reseñas
{
    public class CalificarReseñaDTO
    {
        public string idSerie { get; set; }
        public bool? meGusta { get; set; } = null;
        public bool? meDisgusta { get; set; } = null;
    }
}
