namespace ProyectoElectivaV.Model.DbConfiguration
{
    public class ProyectoElectivaVDBSettings : IProyectoElectivaVDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName {  get; set; }
        public string? UsuariosCollectionname { get; set; }
        public string? SeriesCollectionname {  get; set; }
    }

    public interface IProyectoElectivaVDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string? UsuariosCollectionname { get; set; }
        string? SeriesCollectionname { get; set; }
    }
}
