using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSConecFM
{
    [Serializable]
    public class requestTimbrarCFDI
    {
        public string text2CFDI;
        public string UserID;
        public string UserPass;
        public string emisorRFC;
        public Boolean generarCBB;
        public Boolean generarTXT;
        public Boolean generarPDF;
        public string urlTimbrado;

        public requestTimbrarCFDI()
        {
            // Configuración inicial para la conexion con el servios SOAP de Timbrado
            this.UserID = "UsuarioPruebasWS";
            this.UserPass = "b9ec2afa3361a59af4b4d102d3f704eabdf097d4";
            this.emisorRFC = "ESI920427886";
            this.generarCBB = false;
            this.generarPDF = true;
            this.generarTXT = false;
            this.urlTimbrado = "https://t1demo.facturacionmoderna.com/timbrado/soap";
        }
    }
}