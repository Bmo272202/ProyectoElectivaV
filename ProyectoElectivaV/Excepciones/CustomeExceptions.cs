namespace ProyectoElectivaV.Excepciones
{
    public class CustomeExceptions : Exception
    {
        public int StatusCode { get; }
        public static void enviarAlerta(string subjet, string contenido){

            return;
        }
        public CustomeExceptions(string mensaje, int statusCode) : base(mensaje)
        {
            StatusCode = statusCode;
            enviarAlerta("Opps!", mensaje);
        }
    }
}
