using System.Net;

namespace SegurosAPI.Servicio.ManejadorError
{
    public class ManejadorException:Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object Errores { get; } 
        public ManejadorException(HttpStatusCode statusCode, object errores = null) {
            StatusCode = statusCode;
            Errores = errores;
        }
    }
}
