/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Retail Services                   *
*  Namespace : Empiria.Retail.Presentation                      Assembly : Empiria.Retail.dll                *
*  Type      : WebStoreGlobal                                   Pattern  : Global ASP .NET Class             *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Global.asax page class handler used in Web Store solutions.                                   *
*                                                                                                            *
********************************* Copyright (c) 2008-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Presentation.Web;

namespace Empiria.Industries.Retail.Presentation {

  /// <summary>Global.asax page class handler used in Web Store solutions.</summary>
  public class WebStoreGlobal : Global {

    protected void Application_BeginRequest(object sender, EventArgs e) {
      string path = Request.Path;

      if (path.Contains("/products.category/")) {
        string category = path.Substring(path.LastIndexOf("/") + 1).Replace(".aspx", String.Empty);
        Context.RewritePath("~/products/product.category.aspx?name=" + category);
        return;
      }
      if (path.Contains("/vehicles.management/Vehiculos-")) {
        if (!String.IsNullOrEmpty(Request.QueryString["id"])) {
          Context.RewritePath("~/vehicles.management/vehicle.selector.aspx?id=" + Request.QueryString["id"]);
          return;
        }
        if (String.IsNullOrEmpty(Request.QueryString["year"])) {
          string year = path.Substring(path.Length - 9, 4);
          Context.RewritePath("~/vehicles.management/vehicle.selector.aspx?year=" + year);
          return;
        }
        if (String.IsNullOrEmpty(Request.QueryString["modelId"])) {
          string querystring = "year=" + Request.QueryString["year"] + "&makeId=" + Request.QueryString["makeId"];
          Context.RewritePath("~/vehicles.management/vehicle.selector.aspx?" + querystring);
          return;
        }
        if (String.IsNullOrEmpty(Request.QueryString["id"])) {
          string querystring = "year=" + Request.QueryString["year"] + "&makeId=" + Request.QueryString["makeId"];
          querystring += "&modelId=" + Request.QueryString["modelId"];
          Context.RewritePath("~/vehicles.management/vehicle.selector.aspx?" + querystring);
          return;
        }
      }
      if (path.Contains("/buscar.por.marca/")) {
        string brand = path.Substring(path.LastIndexOf("/") + 1).Replace(".aspx", String.Empty);
        Context.RewritePath("~/products/brand.categories.aspx?brand=" + brand);
        return;
      }
    }

  } // WebStoreGlobal

} //namespace Empiria.Industries.Retail.Presentation
