using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Xml;
using System.ServiceModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;


namespace WSConecFM
{
    public class Timbrado
    {
        public Resultados Timbrar(string layout, WSConecFM.requestTimbrarCFDI RequestTimbrarCFDI) 
        {
            Resultados result = new Resultados();
            
            try
            {
              if (File.Exists(layout)) {
                StreamReader objReader = new StreamReader(layout, Encoding.UTF8);
                layout = objReader.ReadToEnd();
                objReader.Close();
              }

                // Codificar a base 64 el layout
                layout = Convert.ToBase64String(Encoding.UTF8.GetBytes(layout));

                // Agregar el XML codificado en base64 a la peticion SOAP
                RequestTimbrarCFDI.text2CFDI = layout;

                //  Conexion con el WS de Facturacion Moderna
                BasicHttpBinding binding = new BasicHttpBinding();
                setBinding(binding);

                // Direccion del servicio SOAP de Prueba
                EndpointAddress endpoint = new EndpointAddress(RequestTimbrarCFDI.urlTimbrado);

                // Crear instancia al servisio SOAP de Timbrado
                WSLayoutFacturacionModerna.Timbrado_ManagerPort WSFModerna = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(binding, endpoint);

                Empiria.Messaging.Publisher.Publish("YY Before Request");

                // Ejecutar servicio de Timbrado
                Object objResponse = WSFModerna.requestTimbrarCFDI(new object[] { RequestTimbrarCFDI } );

                Empiria.Messaging.Publisher.Publish("01 After Request ");

                if (objResponse != null)
                {
                    Empiria.Messaging.Publisher.Publish("02 Not NULL ");

                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration;
                    XmlElement xmlElementBody;
                    xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "uft-8", "no");
                    xmlElementBody = xmlDoc.CreateElement("Container");
                    xmlDoc.InsertBefore(xmlElementBody, xmlDoc.DocumentElement);
                    XmlElement xmlParentNode;
                    xmlParentNode = xmlDoc.CreateElement("responseSoap");
                    xmlDoc.DocumentElement.PrependChild(xmlParentNode);
                    XmlNode[] nodosXmlResponse = (XmlNode[])objResponse;
                    foreach (XmlNode nodo in nodosXmlResponse)
                    {
                        if (nodo.InnerText.Length >= 1)
                        {
                            XmlElement xmlElemetResponse;
                            xmlElemetResponse = xmlDoc.CreateElement(nodo.Name.ToString());
                            XmlText xmlTextNode;
                            xmlTextNode = xmlDoc.CreateTextNode(nodo.InnerText.ToString());
                            xmlParentNode.AppendChild(xmlElemetResponse);
                            xmlElemetResponse.AppendChild(xmlTextNode);
                        }
                    }

                    //-->>Accedemos a los nodos de la respuesta del xml para obenter los valores retornados en base64 (xml, pdf, cbb, txt)
                    XmlElement xmlElementCFDI;
                    //-->>Xml certificado (CFDI)
                    xmlElementCFDI = (XmlElement)xmlDoc.GetElementsByTagName("xml").Item(0);

                    // Obtener UUID del Comprobante
                    XmlDocument cfdiXML = new XmlDocument();
                    byte[] binary = Convert.FromBase64String(xmlElementCFDI.InnerText);
                    String strOriginal = System.Text.Encoding.UTF8.GetString(binary);
                    cfdiXML.LoadXml(strOriginal);
                    XmlElement xmlElementTimbre;
                    xmlElementTimbre = (XmlElement)cfdiXML.GetElementsByTagName("tfd:TimbreFiscalDigital").Item(0);
                    result.uuid = xmlElementTimbre.GetAttribute("UUID");

                    result.xmlBase64 = xmlElementCFDI.InnerText;

                    //FileStream stream = new FileStream(pathFile + "\\" + uuid + ".xml", FileMode.Create); 
                    //BinaryWriter writerBinary = new BinaryWriter(stream);
                    //writerBinary.Write(Convert.FromBase64String(xmlElementCFDI.InnerText));
                    //writerBinary.Close();
                    //if (!File.Exists(pathFile + "\\" + uuid + ".xml"))
                    //{
                    //    result.code = "T000";
                    //    result.message = "Error: El comprobante en XML no se pudo escribir en " + pathFile + "\\" + uuid + ".xml";
                    //    result.status = false;
                    //    return result;
                    //}

                    //-->>Representación impresa del CFDI en formato PDF
                    if (RequestTimbrarCFDI.generarPDF) 
                    {
                        XmlElement xmlElementPDF = (XmlElement)xmlDoc.GetElementsByTagName("pdf").Item(0);
                        result.pdfBase64 = xmlElementPDF.InnerText;
                        ////-->>Almacenamiento del Comprobante en PDF
                        //stream = new FileStream(pathFile + "\\" + uuid + ".pdf", FileMode.Create);
                        //writerBinary = new BinaryWriter(stream);
                        //writerBinary.Write(Convert.FromBase64String(xmlElementPDF.InnerText));
                        //writerBinary.Close();
                        //if (!File.Exists(pathFile + "\\" + uuid + ".pdf"))
                        //{
                        //    result.code = "T001";
                        //    result.message = "Error: El comprobante en PDF no se pudo escribir en " + pathFile + "\\" + uuid + ".pdf";
                        //    result.status = false;
                        //    return result;
                        //}
                    }

                    //-->>Representación impresa del CFDI en formato TXT
                    if (RequestTimbrarCFDI.generarTXT)
                    {
                        XmlElement xmlElementTXT = (XmlElement)xmlDoc.GetElementsByTagName("txt").Item(0);
                        result.txtBase64 = xmlElementTXT.InnerText;
                        //-->>Almacenamiento del Comprobante en PDF
                        //stream = new FileStream(pathFile + "\\" + uuid + ".txt", FileMode.Create);
                        //writerBinary = new BinaryWriter(stream);
                        //writerBinary.Write(Convert.FromBase64String(xmlElementTXT.InnerText));
                        //writerBinary.Close();
                        //if (!File.Exists(pathFile + "\\" + uuid + ".txt"))
                        //{
                        //    result.code = "T002";
                        //    result.message = "Error: El comprobante en TXT no se pudo escribir en " + pathFile + "\\" + uuid + ".txt";
                        //    result.status = false;
                        //    return result;
                        //}
                    }

                    //-->>Representación impresa del CFDI en formato PNG
                    if (RequestTimbrarCFDI.generarCBB)
                    {
                        XmlElement xmlElementCBB = (XmlElement)xmlDoc.GetElementsByTagName("png").Item(0);
                        result.cbbBase64 = xmlElementCBB.InnerText;
                        ////-->>Almacenamiento del Comprobante en PNG
                        //stream = new FileStream(pathFile + "\\" + uuid + ".png", FileMode.Create);
                        //writerBinary = new BinaryWriter(stream);
                        //writerBinary.Write(Convert.FromBase64String(xmlElementCBB.InnerText));
                        //writerBinary.Close();
                        //if (!File.Exists(pathFile + "\\" + uuid + ".png"))
                        //{
                        //    result.code = "T003";
                        //    result.message = "Error: El comprobante en PNG no se pudo escribir en " + pathFile + "\\" + uuid + ".png";
                        //    result.status = false;
                        //    return result;
                        //}
                    }
                    result.code = "T000";
                    result.message = "Comprobante Generado con exito";
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "T00N";
                    result.message = "El servicio de timbrado respondio con NULL";
                    result.status = false;
                    return result;
                }
            }
            catch(Exception e)
            {
                result.code = "" + e.HResult + "";
                result.message = "Error: " + e.Message;
                if (e.InnerException != null) {
                  result.message += " InnerException: " + e.InnerException;
                  result.message += " Trace: " + e.StackTrace;
                }
                result.status = false;
                return result;
            }
        }

        private void setBinding(BasicHttpBinding binding)
        {
            // Crear archivo app.config de forma manual
            binding.Name = "Timbrado_ManagerBinding";
            binding.CloseTimeout = System.TimeSpan.Parse("00:01:00");
            binding.OpenTimeout = System.TimeSpan.Parse("00:01:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:01:00");
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 524288;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 524288;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        }
    }

    public class Cancelado
    {
        public Resultados Cancelar(WSConecFM.requestCancelarCFDI RequestCancelarCFDI, string uuid)
        {
            Resultados result = new Resultados();

            try
            {
                RequestCancelarCFDI.uuid = uuid;

                //  Conexion con el WS de Facturacion Moderna
                BasicHttpBinding binding = new BasicHttpBinding();
                setBinding(binding);

                // Direccion del servicio SOAP de Prueba
                EndpointAddress endpoint = new EndpointAddress(RequestCancelarCFDI.urlCancelado);

                // Crear instancia al servisio SOAP de cancelado
                WSLayoutFacturacionModerna.Timbrado_ManagerPort WSFModerna = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(binding, endpoint);

                // Ejecutar servicio de Cancelado
                Object response = WSFModerna.requestCancelarCFDI(RequestCancelarCFDI);
                if (response != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration;
                    XmlElement xmlElementBody;
                    xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "uft-8", "no");
                    xmlElementBody = xmlDoc.CreateElement("Container");
                    xmlDoc.InsertBefore(xmlElementBody, xmlDoc.DocumentElement);
                    XmlElement xmlParentNode;
                    xmlParentNode = xmlDoc.CreateElement("responseSoap");
                    xmlDoc.DocumentElement.PrependChild(xmlParentNode);
                    XmlNode[] nodosXmlResponse = (XmlNode[])response;
                    foreach (XmlNode nodo in nodosXmlResponse)
                    {
                        if (nodo.InnerText.Length >= 1)
                        {
                            XmlElement xmlElemetResponse;
                            xmlElemetResponse = xmlDoc.CreateElement(nodo.Name.ToString());
                            XmlText xmlTextNode;
                            xmlTextNode = xmlDoc.CreateTextNode(nodo.InnerText.ToString());
                            xmlParentNode.AppendChild(xmlElemetResponse);
                            xmlElemetResponse.AppendChild(xmlTextNode);
                        }
                    }
                    XmlElement xmlElementMsg = (XmlElement)xmlDoc.GetElementsByTagName("Message").Item(0);
                    result.message = xmlElementMsg.InnerText;
                    result.code = "C000";
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "C00N";
                    result.message = "El servicio de Cancelado respondio con NULL";
                    result.status = false;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.code = "" + e.HResult + "";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

        private void setBinding(BasicHttpBinding binding)
        {
            // Crear archivo app.config de forma manual
            binding.Name = "Timbrado_ManagerBinding";
            binding.CloseTimeout = System.TimeSpan.Parse("00:01:00");
            binding.OpenTimeout = System.TimeSpan.Parse("00:01:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:01:00");
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 524288;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 524288;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        }
    }

    public class ActivarCancelado
    {
        public Resultados Activacion(WSConecFM.activarCancelacion activarCancelacion)
        {
            Resultados result = new Resultados();
            string cer = activarCancelacion.archivoCer;
            string key = activarCancelacion.archivoKey;
            string clv = activarCancelacion.clave;
            try
            {
                if (File.Exists(cer))
                {
                    X509Certificate2 cert = new X509Certificate2(cer);
                    cer = Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
                    cer = cer.Replace("\n", "");
                } else 
                    // Codificar a base 64 el contenido del certificado
                    cer = Convert.ToBase64String(Encoding.UTF8.GetBytes(cer));
                // Agregar el certificado codificado en base64 a la peticion SOAP
                activarCancelacion.archivoCer = cer;

                if (File.Exists(key))
                {
                    byte[] llavePrivadaBytes = System.IO.File.ReadAllBytes(@key);
                    key = Convert.ToBase64String(llavePrivadaBytes);

                } else
                    // Codificar a base 64 el contenido del archivo  key
                    key = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
                // Agregar el certificado codificado en base64 a la peticion SOAP
                activarCancelacion.archivoKey = key;

                //  Conexion con el WS de Facturacion Moderna
                BasicHttpBinding binding = new BasicHttpBinding();
                setBinding(binding);

                // Direccion del servicio SOAP de Prueba
                EndpointAddress endpoint = new EndpointAddress(activarCancelacion.urlActivarCancelacion);

                // Crear instancia al servisio SOAP de cancelado
                WSLayoutFacturacionModerna.Timbrado_ManagerPort WSFModerna = new WSLayoutFacturacionModerna.Timbrado_ManagerPortClient(binding, endpoint);

                // Ejecutar servicio de Cancelado
                Object response = WSFModerna.activarCancelacion(activarCancelacion);
                if (response != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration;
                    XmlElement xmlElementBody;
                    xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "uft-8", "no");
                    xmlElementBody = xmlDoc.CreateElement("Container");
                    xmlDoc.InsertBefore(xmlElementBody, xmlDoc.DocumentElement);
                    XmlElement xmlParentNode;
                    xmlParentNode = xmlDoc.CreateElement("responseSoap");
                    xmlDoc.DocumentElement.PrependChild(xmlParentNode);
                    XmlNode[] nodosXmlResponse = (XmlNode[])response;
                    foreach (XmlNode nodo in nodosXmlResponse)
                    {
                        if (nodo.InnerText.Length >= 1)
                        {
                            XmlElement xmlElemetResponse;
                            xmlElemetResponse = xmlDoc.CreateElement(nodo.Name.ToString());
                            XmlText xmlTextNode;
                            xmlTextNode = xmlDoc.CreateTextNode(nodo.InnerText.ToString());
                            xmlParentNode.AppendChild(xmlElemetResponse);
                            xmlElemetResponse.AppendChild(xmlTextNode);
                        }
                    }
                    XmlElement xmlElementMsg = (XmlElement)xmlDoc.GetElementsByTagName("mensaje").Item(0);
                    XmlElement xmlElementCode = (XmlElement)xmlDoc.GetElementsByTagName("codigo").Item(0);
                    result.message = xmlElementMsg.InnerText;
                    result.code = xmlElementCode.InnerText;
                    result.status = true;
                    return result;
                }
                else
                {
                    result.code = "C00N";
                    result.message = "El servicio de Cancelado respondio con NULL";
                    result.status = false;
                    return result;
                }
            }
            catch (Exception e)
            {
                result.code = "" + e.HResult + "";
                result.message = "Error: " + e.Message;
                result.status = false;
                return result;
            }
        }

        private void setBinding(BasicHttpBinding binding)
        {
            // Crear archivo app.config de forma manual
            binding.Name = "Timbrado_ManagerBinding";
            binding.CloseTimeout = System.TimeSpan.Parse("00:01:00");
            binding.OpenTimeout = System.TimeSpan.Parse("00:01:00");
            binding.ReceiveTimeout = System.TimeSpan.Parse("00:10:00");
            binding.SendTimeout = System.TimeSpan.Parse("00:01:00");
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 524288;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 524288;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 8192;
            binding.ReaderQuotas.MaxArrayLength = 16384;
            binding.ReaderQuotas.MaxBytesPerRead = 4096;
            binding.ReaderQuotas.MaxNameTableCharCount = 16384;
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Transport.Realm = "";
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
        }
    }
}