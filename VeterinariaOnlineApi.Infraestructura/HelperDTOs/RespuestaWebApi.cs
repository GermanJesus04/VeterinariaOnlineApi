using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeterinariaOnlineApi.Infraestructura.HelperDTOs
{
    public class RespuestaWebApi <T>
    {
        public bool Exitoso {  get; set; }
        public string Mensaje { get; set; }
        public T Data {  get; set; }
    }
}
