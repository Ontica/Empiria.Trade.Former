/* Empiria® Customers Components 2013 ************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Industries Framework                    System   : Automotive Industry Components    *
*  Namespace : Empiria.Customers.Pineda                         Assembly : Empiria.Customers.Pineda.dll      *
*  Type      : FacturaXmlSAT                                    Pattern  : Storage Item                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Describes a customer.                                                                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Data;
using System.Xml;

using Empiria.Contacts;
using Empiria.SupplyNetwork.Data;

namespace Empiria.SupplyNetwork {

  /// <summary>Describes a customer</summary>

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
      DataView view = SupplyOrdersData.GetBills(fromDate, toDate, "[BillType] IN('B','G') AND [BillStatus] <> 'X' AND " +
                                                "[CancelationTime] > '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      string fileContents = GetReportFileSection(view, "1", "I");   // Active bills

      view = SupplyOrdersData.GetBills(DateTime.Parse("31/12/2011"), fromDate.AddSeconds(-0.5),
                                   "[BillType] = 'B' AND [BillStatus] = 'C' AND [CancelationTime] >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND " +
                                   "[CancelationTime] <= '" + toDate.ToString("yyyy-MM-dd HH:mm:ss") + "'");

      fileContents += GetReportFileSection(view, "0", "I");         // Canceled bills

      view = SupplyOrdersData.GetBills(fromDate, toDate, "[BillType] IN ('C','L') AND [BillStatus] = 'A'");

      fileContents += GetReportFileSection(view, "1", "E");         // Active credit notes

      view = SupplyOrdersData.GetBills(DateTime.Parse("31/12/2011"), fromDate.AddSeconds(-0.5),
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
      AppendXmlAttribute(xml, node, "LugarExpedicion", bill.ExpeditionPlace);
      AppendXmlAttribute(xml, node, "NumCtaPago", bill.PaymentAccount);
      AppendXmlAttribute(xml, node, "tipoDeComprobante", bill.BillTypeFiscalName);
      AppendXmlAttribute(xml, node, "sello", bill.DigitalSign);
      AppendXmlAttribute(xml, node, "xmlns", "http://www.sat.gob.mx/cfd/2");
      AppendXmlAttribute(xml, node, "xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

      attribute = xml.CreateAttribute("xsi:schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
      attribute.Value = "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet";
      node.Attributes.Append(attribute);
    }

    private void AppendXmlAttribute(XmlDocument xml, XmlNode node, string attributeName, string attributeValue) {
      attributeValue = Bill.CleanXmlBillItem(attributeValue);

      if (String.IsNullOrWhiteSpace(attributeValue)) {
        return;
      }
      XmlAttribute attribute = xml.CreateAttribute(attributeName);
      attribute.Value = attributeValue;
      node.Attributes.Append(attribute);
    }

    private void CreateEmisorNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "Emisor", String.Empty);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "rfc", "ARP9706105W2");
      AppendXmlAttribute(xml, node, "nombre", "AUTO REFACCIONES PINEDA, S.A. de C.V.");

      XmlNode address = xml.CreateNode(XmlNodeType.Element, "DomicilioFiscal", String.Empty);

      node.AppendChild(address);

      AppendXmlAttribute(xml, address, "calle", "Avenida Instituto Politécnico Nacional");
      AppendXmlAttribute(xml, address, "noExterior", "5015");
      AppendXmlAttribute(xml, address, "colonia", "Capultitlán");
      AppendXmlAttribute(xml, address, "municipio", "Gustavo A. Madero");
      AppendXmlAttribute(xml, address, "estado", "D.F.");
      AppendXmlAttribute(xml, address, "pais", "México");
      AppendXmlAttribute(xml, address, "codigoPostal", "07370");

      XmlNode fiscalRegulation = xml.CreateNode(XmlNodeType.Element, "RegimenFiscal", String.Empty);
      node.AppendChild(fiscalRegulation);
      AppendXmlAttribute(xml, fiscalRegulation, "Regimen", bill.FiscalRegulation);
    }

    private void CreateReceiverNode(XmlDocument xml) {
      XmlNode node = null;

      node = xml.CreateNode(XmlNodeType.Element, "Receptor", String.Empty);
      xml.DocumentElement.AppendChild(node);

      AppendXmlAttribute(xml, node, "rfc", bill.Order.Customer.FormattedTaxTag);
      AppendXmlAttribute(xml, node, "nombre", bill.Order.Customer.FullName);

      XmlNode address = xml.CreateNode(XmlNodeType.Element, "Domicilio", String.Empty);

      node.AppendChild(address);

      AppendXmlAttribute(xml, address, "calle", bill.Order.Customer.Address.Street);
      AppendXmlAttribute(xml, address, "noExterior", bill.Order.Customer.Address.ExtNumber);
      AppendXmlAttribute(xml, address, "noInterior", bill.Order.Customer.Address.IntNumber);
      AppendXmlAttribute(xml, address, "colonia", bill.Order.Customer.Address.Borough);
      AppendXmlAttribute(xml, address, "municipio", bill.Order.Customer.Address.Municipality);
      AppendXmlAttribute(xml, address, "estado", bill.Order.Customer.Address.State);
      AppendXmlAttribute(xml, address, "pais", "México");
      AppendXmlAttribute(xml, address, "codigoPostal", bill.Order.Customer.Address.ZipCode);
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
        if (bill.BillType == SupplyNetwork.BillType.GlobalBill) {
          AppendXmlAttribute(xml, itemNode, "descripcion", "Factura global mensual. Folios: " + bill.NotOrderData.TicketNumbers);
        } else if (bill.BillType == SupplyNetwork.BillType.GlobalCreditNote) {
          AppendXmlAttribute(xml, itemNode, "descripcion", "Factura global mensual. Devoluciones de los folios: " + bill.NotOrderData.TicketNumbers);
        }
        AppendXmlAttribute(xml, itemNode, "valorUnitario", bill.NotOrderData.SubTotal.ToString("0.00"));
        AppendXmlAttribute(xml, itemNode, "importe", bill.NotOrderData.SubTotal.ToString("0.00"));

        return;
      }

      SupplyOrderItemList items = bill.Order.Items;
      foreach (SupplyOrderItem item in items) {
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

      SupplyOrderItemList items = bill.Order.Items;
      foreach (SupplyOrderItem item in items) {
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

    private static Tuple<decimal, decimal> GetXmlFileBillTotals(string xmlData) {
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

} // namespace Empiria.SupplyNetwork