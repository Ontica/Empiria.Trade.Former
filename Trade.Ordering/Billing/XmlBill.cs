/* Empiria Customers Components 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : FacturaXmlSAT                                    Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
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
                                   "[BillType] = 'B' AND [BillStatus] = 'C' AND [CancelationTime] >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
                                   "[CancelationTime] <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      fileContents += GetReportFileSection(view, "0", "I");         // Canceled bills

      view = BillingData.GetBills(fromDate, toDate, "[BillType] IN ('C','L') AND [BillStatus] = 'A'");

      fileContents += GetReportFileSection(view, "1", "E");         // Active credit notes

      view = BillingData.GetBills(DateTime.Parse("31/12/2011"), fromDate.AddSeconds(-0.5),
                                  "[BillType] IN ('C','L') AND [BillStatus] = 'C' AND [CancelationTime] >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
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

      return xml;
    }

    private void CreateMainNode(XmlDocument xml) {
      XmlNode node = null;
      XmlAttribute attribute = null;

      xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", String.Empty));

      node = xml.CreateNode(XmlNodeType.Element, "Comprobante", String.Empty);
      xml.AppendChild(node);

      AppendXmlAttribute(xml, node, "version", bill.Version);
      AppendXmlAttribute(xml, node, "noCertificado", bill.CertificateNumber);
      AppendXmlAttribute(xml, node, "serie", bill.SerialNumber);
      AppendXmlAttribute(xml, node, "folio", bill.Number);
      AppendXmlAttribute(xml, node, "noAprobacion", bill.ApprovalNumber);
      AppendXmlAttribute(xml, node, "anoAprobacion", bill.ApprovalYear.ToString());
      AppendXmlAttribute(xml, node, "fecha", bill.IssuedTime.ToString(@"yyyy-MM-dd\THH:mm:ss"));
      AppendXmlAttribute(xml, node, "subTotal", bill.SubTotal.ToString("0.00"));
      AppendXmlAttribute(xml, node, "total", bill.Total.ToString("0.00"));
      AppendXmlAttribute(xml, node, "formaDePago", bill.PaymentMode);
      AppendXmlAttribute(xml, node, "metodoDePago", bill.PaymentCondition);
      AppendXmlAttribute(xml, node, "LugarExpedicion", bill.IssuerData.IssuePlace);
      AppendXmlAttribute(xml, node, "NumCtaPago", bill.PaymentAccount);
      AppendXmlAttribute(xml, node, "tipoDeComprobante", bill.BillTypeFiscalName);
      AppendXmlAttribute(xml, node, "sello", bill.DigitalSign);
      AppendXmlAttribute(xml, node, "xmlns", "http://www.sat.gob.mx/cfd/2");
      AppendXmlAttribute(xml, node, "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

      attribute = xml.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
      attribute.Value = "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet";
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

      node = xml.CreateNode(XmlNodeType.Element, "Emisor", String.Empty);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "rfc", supplier.FormattedTaxIDNumber);                 //  "ARP9706105W2" "TUMG620310R95"
      AppendXmlAttribute(xml, node, "nombre", supplier.FullName);                     //  "AUTO REFACCIONES PINEDA, S.A. de C.V."

      XmlNode address = xml.CreateNode(XmlNodeType.Element, "DomicilioFiscal", String.Empty);

      node.AppendChild(address);

      AppendXmlAttribute(xml, address, "calle", supplier.Address.Street);             //  "Avenida Instituto Politécnico Nacional"
      AppendXmlAttribute(xml, address, "noExterior", supplier.Address.ExtNumber);     //  "5015"
      AppendXmlAttribute(xml, address, "colonia", supplier.Address.Borough);          //  "Capultitlán"
      AppendXmlAttribute(xml, address, "municipio", supplier.Address.Municipality);   //  "Gustavo A. Madero"
      AppendXmlAttribute(xml, address, "estado", supplier.Address.State);             //  "D.F."
      AppendXmlAttribute(xml, address, "pais", "México");
      AppendXmlAttribute(xml, address, "codigoPostal", supplier.Address.ZipCode);     //  "07370"

      XmlNode fiscalRegulation = xml.CreateNode(XmlNodeType.Element, "RegimenFiscal", String.Empty);
      node.AppendChild(fiscalRegulation);
      AppendXmlAttribute(xml, fiscalRegulation, "Regimen", bill.IssuerData.FiscalRegimen);
    }

    private void CreateReceiverNode(XmlDocument xml) {
      Contact customer = bill.Order.Customer;
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "Receptor", String.Empty);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "rfc", customer.FormattedTaxIDNumber);
      AppendXmlAttribute(xml, node, "nombre", customer.FullName);

      XmlNode address = xml.CreateNode(XmlNodeType.Element, "Domicilio", String.Empty);

      node.AppendChild(address);

      AppendXmlAttribute(xml, address, "calle", customer.Address.Street);
      AppendXmlAttribute(xml, address, "noExterior", customer.Address.ExtNumber);
      AppendXmlAttribute(xml, address, "noInterior", customer.Address.IntNumber);
      AppendXmlAttribute(xml, address, "colonia", customer.Address.Borough);
      AppendXmlAttribute(xml, address, "municipio", customer.Address.Municipality);
      AppendXmlAttribute(xml, address, "estado", customer.Address.State);
      AppendXmlAttribute(xml, address, "pais", "México");
      AppendXmlAttribute(xml, address, "codigoPostal", customer.Address.ZipCode);
    }

    private void CreateItemsNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "Conceptos", String.Empty);
      xml.DocumentElement.AppendChild(node);

      if (bill.NotOrderData != null) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "Concepto", String.Empty);

        node.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "cantidad", "1.0");
        AppendXmlAttribute(xml, itemNode, "unidad", "No identificado");
        if (bill.BillType == BillType.GlobalBill) {
          AppendXmlAttribute(xml, itemNode, "descripcion", "Factura global mensual. Folios: " + bill.NotOrderData.TicketNumbers);
        } else if (bill.BillType == BillType.GlobalCreditNote) {
          AppendXmlAttribute(xml, itemNode, "descripcion", "Factura global mensual. Devoluciones de los folios: " + bill.NotOrderData.TicketNumbers);
        }
        AppendXmlAttribute(xml, itemNode, "valorUnitario", bill.NotOrderData.SubTotal.ToString("0.00"));
        AppendXmlAttribute(xml, itemNode, "importe", bill.NotOrderData.SubTotal.ToString("0.00"));

        return;
      }

      var orderItems = bill.Order.Items;
      foreach (var item in orderItems) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "Concepto", String.Empty);

        node.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "cantidad", item.Quantity.ToString("0.0"));
        AppendXmlAttribute(xml, itemNode, "unidad", item.PresentationUnit.Name);
        if (!String.IsNullOrWhiteSpace(item.Concept)) {
          AppendXmlAttribute(xml, itemNode, "descripcion", item.Concept);
        } else {
          AppendXmlAttribute(xml, itemNode, "descripcion", item.Product.Name);
        }
        AppendXmlAttribute(xml, itemNode, "valorUnitario", item.ProductDiscountUnitPrice.ToString("0.00"));
        AppendXmlAttribute(xml, itemNode, "importe", item.ProductSubTotalBeforeTaxes.ToString("0.00"));
      } // foreach

    }

    private void CreateTaxesNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "Impuestos", String.Empty);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "totalImpuestosTrasladados", (bill.Total - bill.SubTotal).ToString("0.00"));

      XmlNode taxes = xml.CreateNode(XmlNodeType.Element, "Traslados", String.Empty);
      node.AppendChild(taxes);

      if (bill.NotOrderData != null) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "Traslado", String.Empty);

        taxes.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "impuesto", "IVA");
        AppendXmlAttribute(xml, itemNode, "tasa", "16.00");
        AppendXmlAttribute(xml, itemNode, "importe", (bill.Total - bill.SubTotal).ToString("0.00"));

        return;
      }

      var orderItems = bill.Order.Items;
      foreach (var item in orderItems) {
        XmlNode itemNode = xml.CreateNode(XmlNodeType.Element, "Traslado", String.Empty);

        taxes.AppendChild(itemNode);

        AppendXmlAttribute(xml, itemNode, "impuesto", "IVA");
        AppendXmlAttribute(xml, itemNode, "tasa", "16.00");
        AppendXmlAttribute(xml, itemNode, "importe", item.Taxes.ToString("0.00"));
      }
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

    static private Tuple<decimal, decimal> GetXmlFileBillTotals(string xmlData) {
      decimal total = 0m;
      decimal taxes = 0m;

      XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(xmlData));
      while (reader.Read()) {
        if (reader.NodeType == XmlNodeType.Element && reader.Name == "Comprobante") {
          if (reader.MoveToAttribute("total")) {
            total = Convert.ToDecimal(reader.Value);
          }
          if (reader.MoveToAttribute("subTotal")) {
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
