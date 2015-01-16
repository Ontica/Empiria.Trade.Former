/* Empiria Industries Framework 2015 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Retail Industry Components        *
*  Namespace : Empiria.Industries.Retail.Presentation           Assembly : Empiria.Industries.Retail.dll     *
*  Type      : WebStoreMasterPage                               Pattern  : Model View Controller             *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract type that represents a master page that serves as template and container             *
*              for web store pages.                                                                          *
*                                                                                                            *
********************************* Copyright (c) 2008-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Empiria.Presentation.Web;

namespace Empiria.Industries.Retail.Presentation {

  /// <summary>Abstract type that represents a master page that serves as template and container for
  /// web store pages.</summary>
  public abstract class WebStoreMasterPage : MasterWebPage {

    #region Fields

    #endregion Fields

    #region Public properties

    public WebStoreSession WebStoreSession {
      get {
        if (Session["EmpiriaWebStoreSession@" + ExecutionServer.LicenseName] == null) {
          Session["EmpiriaWebStoreSession@" + ExecutionServer.LicenseName] = new WebStoreSession(Empiria.ExecutionServer.CurrentSessionToken);
        }
        return (WebStoreSession) Session["EmpiriaWebStoreSession@" + ExecutionServer.LicenseName];
      }
    }

    #endregion Public properties

    #region Protected properties

    protected string AppPath {
      get {
        return (Request.ApplicationPath == "/") ? String.Empty : Request.ApplicationPath;
      }
    }

    protected new WebStorePage Page {
      get { return (WebStorePage) base.Page; }
    }

    #endregion Protected properties

    #region Protected methods

    //protected void LoadProductApplianceCombo(HtmlSelect comboControl,
    //                                         string emptyListItemName, string firstListItemName) {
    //  comboControl.Items.Clear();
    //  if (this.WebStoreSession.ProductAppliances.Count == 0) {
    //    comboControl.Items.Add(emptyListItemName);
    //    return;
    //  }
    //  foreach (KeyValuePair<string, Empiria.Products.IProductAppliance> kvp in this.WebStoreSession.ProductAppliances) {
    //    var item = new System.Web.UI.WebControls.ListItem(kvp.Value.Name, kvp.Value.Id.ToString());
    //    comboControl.Items.Add(item);
    //  }
    //  comboControl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(firstListItemName, String.Empty));
    //  if (this.WebStoreSession.SelectedProductAppliance != null) {
    //    comboControl.Value = this.WebStoreSession.SelectedProductAppliance.Id.ToString();
    //  }
    //}

    #endregion Protected methods

  } // class WebStoreMasterPage

} // namespace Empiria.Industries.Retail.Presentation
