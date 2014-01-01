using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSConecFM
{
    [Serializable]
    public class activarCancelacion
    {
        public string archivoKey;
        public string UserID;
        public string UserPass;
        public string emisorRFC;
        public string archivoCer;
        public string clave;
        public string urlActivarCancelacion;

        public activarCancelacion() 
        {
            // Configuración inicial para la conexion con el servios SOAP de Timbrado
            this.UserID = "UsuarioPruebasWS";
            this.UserPass = "b9ec2afa3361a59af4b4d102d3f704eabdf097d4";
            this.emisorRFC = "ESI920427886";
            this.urlActivarCancelacion = "https://t1demo.facturacionmoderna.com/timbrado/soap";
        }

    }
}
