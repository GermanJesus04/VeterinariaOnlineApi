using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Infraestructura.HelperDTOs
{
    public class ExcepcionPeticionApi : Exception
    {
        private int _codigoError;
        public int CodigoError => _codigoError;

        public ExcepcionPeticionApi() {}

        public ExcepcionPeticionApi(string mensaje, int CodigoError): base(message: mensaje)
        {
            _codigoError = CodigoError;
        }

        public ExcepcionPeticionApi(string mensaje, int CodigoError, Exception excepcionInterna): base(mensaje)
        {
            _codigoError = CodigoError;
        }

    }
}
