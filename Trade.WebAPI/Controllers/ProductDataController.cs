/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Trade Web API                     *
*  Namespace : Empiria.Trade.WebApi                             Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : ProductDataController                            Pattern  : Web API                           *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Services to manage brands, product types, units, and other product related information.       *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Products;

namespace Empiria.Trade.WebApi {

  /// <summary>Services to manage brands, product types, units, and other product related information.</summary>
  public class ProductDataController : WebApiController {

    #region Brand APIs

    [HttpGet]
    [Route("v1/product-data/brands")]
    public CollectionModel GetBrands([FromUri] string search = "") {
      try {
        var list = Brand.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Brand APIs


    #region ProductCategory APIs

    [HttpGet]
    [Route("v1/product-data/product-categories")]
    public CollectionModel GetProductCategories([FromUri] string search = "") {
      try {
        var list = ProductCategory.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion ProductCategory APIs


    #region ProductTerm APIs

    [HttpGet]
    [Route("v1/product-data/product-terms")]
    public CollectionModel GetProductTerm([FromUri] string search = "") {
      try {
        var list = ProductTerm.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion ProductTerm APIs

  }  // class ProductDataController

}  // namespace Empiria.Trade.WebApi
