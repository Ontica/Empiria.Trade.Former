/* Empiria Industries Framework 2014 *************************************************************************
*                                                                                                            *
*  Solution  : Empiria Industries Framework                     System   : Retail Industry Components        *
*  Namespace : Empiria.Industries.Retail                        Assembly : Empiria.Industries.Retail.dll     *
*  Type      : WebStoreSession                                  Pattern  : Standard Class                    *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract type that represents a web page for online stores.                                   *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;
using Empiria.Industries.Retail.Data;
using Empiria.Products;

namespace Empiria.Industries.Retail {

  public class WebStoreSession {

    #region Fields

    private string sessionToken = String.Empty;
    private string productSearch = String.Empty;
    private string productGroupName = String.Empty;
    private SortedList<string, IProductAppliance> productApplianceList = new SortedList<string, IProductAppliance>();
    private IProductAppliance selectedProductAppliance = null;

    #endregion Fields

    #region Constructors and parsers

    public WebStoreSession(string sessionToken) {
      this.sessionToken = sessionToken;
    }

    #endregion Constructors and parsers

    #region Public properties

    public SortedList<string, IProductAppliance> ProductAppliances {
      get { return productApplianceList; }
    }

    public IProductAppliance SelectedProductAppliance {
      get { return selectedProductAppliance; }
    }

    #endregion Public properties

    #region Public methods

    public string GetLastProductSearchKeywords() {
      return productSearch;
    }

    public void PushProductsAppliance(IProductAppliance productAppliance) {
      if (!productApplianceList.ContainsKey(productAppliance.Name)) {
        productApplianceList.Add(productAppliance.Name, productAppliance);
      }
      selectedProductAppliance = productAppliance;
    }

    public void PushProductSearch(string keywords) {
      productSearch = EmpiriaString.BuildKeywords(keywords);
    }

    public void PushProductGroup(string productGroupName) {
      this.productGroupName = productGroupName;
    }

    public DataView SearchProducts(string keywords) {
      return StoreData.GetProductsDataView(keywords);
    }

    #endregion Public methods

  } // class WebStoreSession

} // namespace Empiria.Industries.Retail
