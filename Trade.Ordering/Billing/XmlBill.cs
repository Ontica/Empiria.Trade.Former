/* Empiria Customers Components 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : FacturaXmlSAT                                    Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Describes a Xml bill.                                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Xml;

using Empiria.Contacts;

using Empiria.Trade.Data;

namespace Empiria.Trade.Billing {

  /// <summary>Describes a Xml bill</summary>
  public class XmlBill {

    #region Fields

    private static string namespaceURI = "http://www.sat.gob.mx/cfd/3";

    private Bill bill = null;

    #endregion Fields

    #region Constructors and parsers

    private XmlBill() {

    }

    static public XmlBill Parse(Bill bill) {
      XmlBill facturaXml = new XmlBill();

      facturaXml.bill = bill;

      return facturaXml;
    }

    static public string CreateReportFile(Contact supplier, DateTime fromDate, DateTime toDate) {
      DataView view = BillingData.GetBills(fromDate, toDate, "[BillType] IN('B','G') AND [BillStatus] <> 'X' AND " +
                                           "[CancelationTime] > '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      string fileContents = GetReportFileSection(view, "1", "I");   // Active bills

      view = BillingData.GetBills(DateTime.Parse("31/12/2011"), fromDate.AddSeconds(-0.5),
                                  "[BillType] = 'B' AND [BillStatus] = 'C' AND [CancelationTime] >= '" +
                                   fromDate.ToString("yyyy-MM-dd") + "' AND " +
                                  "[CancelationTime] <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      fileContents += GetReportFileSection(view, "0", "I");         // Canceled bills

      view = BillingData.GetBills(fromDate, toDate, "[BillType] IN ('C','L') AND [BillStatus] = 'A'");

      fileContents += GetReportFileSection(view, "1", "E");         // Active credit notes

      view = BillingData.GetBills(DateTime.Parse("31/12/2011"), fromDate.AddSeconds(-0.5),
                                  "[BillType] IN ('C','L') AND [BillStatus] = 'C' AND [CancelationTime] >= '" +
                                   fromDate.ToString("yyyy-MM-dd") + "' AND " +
                                  "[CancelationTime] <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      fileContents += GetReportFileSection(view, "0", "E");         // Canceled credit notes

      fileContents = fileContents.TrimEnd(System.Environment.NewLine.ToCharArray());

      string fileName = GetReportFileName(toDate);

      System.IO.File.WriteAllText(fileName, fileContents);

      return fileName;
    }

    #endregion Constructors and parsers

    #region Methods

    private void AppendXmlAttribute(XmlDocument xml, XmlNode node, string attributeName, string attributeValue) {
      attributeValue = Bill.CleanXmlBillItem(attributeValue);

      if (String.IsNullOrWhiteSpace(attributeValue)) {
        return;
      }
      XmlAttribute attribute = xml.CreateAttribute(attributeName);
      attribute.Value = attributeValue;
      node.Attributes.Append(attribute);
    }

    public XmlDocument CreateDocument() {
      XmlDocument xml = new XmlDocument();

      CreateMainNode(xml);
      CreateEmisorNode(xml);
      CreateReceiverNode(xml);
      CreateItemsNode(xml);
      CreateTaxesNode(xml);

      AddSealAttibute(xml);

      return xml;
    }

    private void CreateMainNode(XmlDocument xml) {
      XmlNode node = null;
      XmlAttribute attribute = null;

      xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", String.Empty));

      node = xml.CreateNode(XmlNodeType.Element, "cfdi", "Comprobante", namespaceURI);
      xml.AppendChild(node);

      AppendXmlAttribute(xml, node, "Version", bill.Version);
      AppendXmlAttribute(xml, node, "Certificado", bill.CertificateContent);
      AppendXmlAttribute(xml, node, "NoCertificado", bill.CertificateNumber);
      AppendXmlAttribute(xml, node, "Serie", bill.SerialNumber);
      AppendXmlAttribute(xml, node, "Folio", bill.Number);
      AppendXmlAttribute(xml, node, "Fecha", bill.IssuedTime.ToString(@"yyyy-MM-dd\THH:mm:ss"));
      AppendXmlAttribute(xml, node, "Moneda", "MXN");
      AppendXmlAttribute(xml, node, "SubTotal", bill.SubTotal.ToString("0.00"));
      AppendXmlAttribute(xml, node, "Total", bill.Total.ToString("0.00"));
      AppendXmlAttribute(xml, node, "MetodoPago", bill.PaymentMethod);           // una sola exhibición
      AppendXmlAttribute(xml, node, "FormaPago", bill.PaymentCondition == "NA" ? "" : bill.PaymentCondition);     // efectivo, t. débito, cheque, etc.
      AppendXmlAttribute(xml, node, "LugarExpedicion", bill.IssuerData.IssuePlace);
      AppendXmlAttribute(xml, node, "NumCtaPago", bill.PaymentAccount);
      AppendXmlAttribute(xml, node, "TipoDeComprobante", bill.BillTypeFiscalName);
      AppendXmlAttribute(xml, node, "xmlns:cfdi", namespaceURI);
      AppendXmlAttribute(xml, node, "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

      attribute = xml.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
      attribute.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd";
      node.Attributes.Append(attribute);
    }

    private void CreateEmisorNode(XmlDocument xml) {
      Contact supplier = null;
      if (bill.NotOrderData == null) {
        supplier = bill.Order.Supplier;
      } else {
        supplier = bill.IssuedBy;
      }

      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "cfdi", "Emisor", namespaceURI);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "Rfc", supplier.FormattedTaxTag);                  //  "ARP9706105W2" "TUMG620310R95"
      AppendXmlAttribute(xml, node, "Nombre", supplier.FullName);                      //  "AUTO REFACCIONES PINEDA, S.A. de C.V."
      AppendXmlAttribute(xml, node, "RegimenFiscal", bill.IssuerData.FiscalRegimen);   //  "Clave Régimen Fiscal"
    }

    private void CreateReceiverNode(XmlDocument xml) {
      Contact customer = bill.Order.Customer;
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "cfdi", "Receptor", namespaceURI);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "Rfc", customer.FormattedTaxTag);
      AppendXmlAttribute(xml, node, "Nombre", customer.FullName);
      AppendXmlAttribute(xml, node, "UsoCFDI", "G01");
    }

    private void CreateItemsNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "cfdi", "Conceptos", namespaceURI);
      xml.DocumentElement.AppendChild(node);

      if (bill.NotOrderData != null) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Concepto", namespaceURI);

        node.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "Cantidad", "1.0");
        AppendXmlAttribute(xml, itemNode, "NoIdentificacion", "A-3452");    // Pineda product code
        AppendXmlAttribute(xml, itemNode, "ClaveProdServ", "93131607");     // From Catálogo SAT
        AppendXmlAttribute(xml, itemNode, "ClaveUnidad", "H87");            // H87 == PIEZA
        AppendXmlAttribute(xml, itemNode, "Unidad", "No identificado");
        if (bill.BillType == BillType.GlobalBill) {
          AppendXmlAttribute(xml, itemNode, "Descripcion", "Factura global mensual. Folios: " + bill.NotOrderData.TicketNumbers);
        } else if (bill.BillType == BillType.GlobalCreditNote) {
          AppendXmlAttribute(xml, itemNode, "Descripcion", "Factura global mensual. Devoluciones de los folios: " + bill.NotOrderData.TicketNumbers);
        }
        AppendXmlAttribute(xml, itemNode, "ValorUnitario", bill.NotOrderData.SubTotal.ToString("0.00"));
        AppendXmlAttribute(xml, itemNode, "Importe", bill.NotOrderData.SubTotal.ToString("0.00"));

        return;
      }

      var orderItems = bill.Order.Items;
      foreach (var item in orderItems) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Concepto", namespaceURI);

        node.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "Cantidad", item.Quantity.ToString("0.0"));
        AppendXmlAttribute(xml, itemNode, "NoIdentificacion", "A-3452");    // Pineda product code
        AppendXmlAttribute(xml, itemNode, "ClaveProdServ", "93131607");     // From Catálogo SAT
        AppendXmlAttribute(xml, itemNode, "ClaveUnidad", "H87");            // H87 == PIEZA
        AppendXmlAttribute(xml, itemNode, "Unidad", item.PresentationUnit.Name);
        if (!String.IsNullOrWhiteSpace(item.Concept)) {
          AppendXmlAttribute(xml, itemNode, "Descripcion", item.Concept);
        } else {
          AppendXmlAttribute(xml, itemNode, "Descripcion", item.Product.Name);
        }
        AppendXmlAttribute(xml, itemNode, "ValorUnitario", item.ProductDiscountUnitPrice.ToString("0.00"));
        AppendXmlAttribute(xml, itemNode, "Importe", item.ProductSubTotalBeforeTaxes.ToString("0.00"));

        // Impuestos -> Traslados -> Traslado IVA
        XmlNode taxesNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Impuestos", namespaceURI);
        itemNode.AppendChild(taxesNode);
        XmlNode trasladosNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Traslados", namespaceURI);
        taxesNode.AppendChild(trasladosNode);

        XmlNode taxNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Traslado", namespaceURI);
        trasladosNode.AppendChild(taxNode);

        AppendXmlAttribute(xml, taxNode, "Impuesto", "002");   // 002 == IVA        
        AppendXmlAttribute(xml, taxNode, "TipoFactor", "Tasa");
        AppendXmlAttribute(xml, taxNode, "TasaOCuota", "0.160000");
        AppendXmlAttribute(xml, taxNode, "Base", item.ProductSubTotalBeforeTaxes.ToString("0.00"));
        AppendXmlAttribute(xml, taxNode, "Importe", item.Taxes.ToString("0.00"));

      } // foreach

    }

    private void CreateTaxesNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "cfdi", "Impuestos", namespaceURI);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "TotalImpuestosTrasladados", (bill.Total - bill.SubTotal).ToString("0.00"));

      XmlNode taxes = xml.CreateNode(XmlNodeType.Element, "cfdi", "Traslados", namespaceURI);
      node.AppendChild(taxes);

      if (bill.NotOrderData != null) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Traslado", namespaceURI);

        taxes.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "Impuesto", "002");   // 002 == IVA        
        AppendXmlAttribute(xml, itemNode, "TipoFactor", "Tasa");
        AppendXmlAttribute(xml, itemNode, "TasaOCuota", "0.160000");
        AppendXmlAttribute(xml, itemNode, "Importe", (bill.Total - bill.SubTotal).ToString("0.00"));

        return;
      }

      var orderItems = bill.Order.Items;
      foreach (var item in orderItems) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "cfdi", "Traslado", namespaceURI);

        taxes.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "Impuesto", "002");
        AppendXmlAttribute(xml, itemNode, "TipoFactor", "Tasa");
        AppendXmlAttribute(xml, itemNode, "TasaOCuota", "0.160000");
        AppendXmlAttribute(xml, itemNode, "Importe", item.Taxes.ToString("0.00"));
      }
    }

    private void AddSealAttibute(XmlDocument xml) {
      var billSeal = new BillSeal(xml);

      string signedBillSeal = billSeal.Sign(this.bill.IssuerData);

      XmlNode node = xml.DocumentElement;     // Gets the xml's root node

      // Add attribute with bill seal value
      XmlAttribute attribute = xml.CreateAttribute("Sello");
      attribute.Value = signedBillSeal;
      node.Attributes.Append(attribute);
    }

    static private string GetReportFileName(DateTime toDate) {
      string fileName = "1ARP9706105W2" + (toDate.Month).ToString("00") + (toDate.Year).ToString("0000");

      return @"D:\facturas.reportes\" + fileName + ".txt";
    }

    static private string GetReportFileSection(DataView view, string status, string itemType) {
      const string delimiter = "|";

      string fileContents = String.Empty;
      foreach (DataRowView row in view) {
        string temp = delimiter;

        temp += EmpiriaString.FormatTaxTag((string) row["CustomerTaxIDNumber"]) + delimiter;
        temp += (string) row["BillSerialNumber"] + delimiter;
        temp += (string) row["BillNumber"] + delimiter;
        temp += ((int) row["ApprovalYear"]).ToString() + (string) row["ApprovalNumber"] + delimiter;
        temp += ((DateTime) row["IssuedTime"]).ToString("dd/MM/yyyy HH:mm:ss") + delimiter;
        BillType billType = (BillType) Convert.ToChar(row["BillType"]);

        if (billType == BillType.GlobalBill || billType == BillType.GlobalCreditNote) {
          Tuple<decimal, decimal> totals = GetXmlFileBillTotals((string) row["BillXmlVersion"]);
          temp += (totals.Item1).ToString("0.00") + delimiter;
          temp += (totals.Item2).ToString("0.00") + delimiter;
        } else {
          temp += ((decimal) row["SupplyOrderTotal"]).ToString("0.00") + delimiter;
          temp += ((decimal) row["SupplyOrderTaxes"]).ToString("0.00") + delimiter;
        }
        temp += status + delimiter;
        temp += itemType + delimiter;
        temp += delimiter; // Pedimento
        temp += delimiter; // Fecha Pedimento
        temp += delimiter; // Aduana

        fileContents += temp + System.Environment.NewLine;
      }

      return fileContents;
    }

    private static Tuple<decimal, decimal> GetXmlFileBillTotals(string xmlData) {
      decimal total = 0m;
      decimal taxes = 0m;

      XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(xmlData));
      while (reader.Read()) {
        if (reader.NodeType == XmlNodeType.Element && reader.Prefix == "cfdi" && reader.Name == "Comprobante") {
          if (reader.MoveToAttribute("Total")) {
            total = Convert.ToDecimal(reader.Value);
          }
          if (reader.MoveToAttribute("SubTotal")) {
            taxes = total - Convert.ToDecimal(reader.Value);
          }
          break;
        }
      }
      reader.Close();
      return new Tuple<decimal, decimal>(total, taxes);
    }

    #endregion Methods

  } // class XmlBill

} // namespace Empiria.Trade.Billing
