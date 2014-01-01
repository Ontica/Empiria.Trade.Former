using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSConecFM
{
    public class Resultados
    {
        public string code;
        public string message;
        public Boolean status;
        public string pdfBase64;
        public string xmlBase64;
        public string txtBase64;
        public string cbbBase64;
        public string uuid;

        public Resultados()
        {
            this.code = "0";
            this.message = "";
            this.status = true;
        }
    }
}
