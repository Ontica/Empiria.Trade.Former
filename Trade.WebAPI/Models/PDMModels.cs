/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Trade Web API                       *
*  Namespace : Empiria.Trade.WebApi                           Assembly : Empiria.Trade.WebApi.dll            *
*  Type      : PDMModels                                      Pattern  : Static class                        *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Static type that provides product data management object models for web API consumption.      *
*                                                                                                            *
********************************* Copyright (c) 2009-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Products;

namespace Empiria.Trade.WebApi.Models {

  /// <summary>Static type that provides product data management object models
  /// for web API consumption.</summary>
  static public class PDMModels {

    #region Public methods

    static public object GetProductModel(Product o) {
      return new {
        id = o.Id,
        category = o.ProductTerm.Category,
        subcategory = o.ProductTerm.Subcategory,
        productTerm = o.ProductTerm,
        manufacturer = o.Manufacturer,
        brand = o.Brand,
        partNumber = o.ProductCode,
        name = o.Name,
        searchTags = o.SearchTags,
        description = o.Description,
        notes = o.Notes,

        presentationUnit = o.PresentationUnit,
        contentQty = o.ContentQty,
        contentUnit = o.ContentUnit,

        startDate = o.StartDate,
        lastUpdated = o.LastUpdated,
        manager = o.ProductManager,
        baseProductId = o.BaseProduct.Id
      };
    }

    #endregion Public methods

  }  // class PDMModels

}  // namespace Empiria.Trade.WebApi.Models
