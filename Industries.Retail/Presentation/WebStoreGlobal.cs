/* Empiria Industries Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Retail Industry Components        *
*  Namespace : Empiria.Industries.Retail.Presentation           Assembly : Empiria.Industries.Retail.dll     *
*  Type      : WebStoreGlobal                                   Pattern  : Global ASP .NET Class             *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Global.asax page class handler used in Web Store solutions.                                   *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
