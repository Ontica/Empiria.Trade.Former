/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Trade Web API                     *
*  Namespace : Empiria.Trade.WebApi                             Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : EquivalentProductsController                     Pattern  : Web API                           *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Equivalent products web services for reading and editing.                                     *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Linq;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Products;
using Empiria.Trade.WebApi.Models;

namespace Empiria.Trade.WebApi {

  /// <summary>Equivalent products web services for reading and editing.</summary>
  public class EquivalentProductsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/product-data/products/{productId}/equivalents")]
    public CollectionModel GetEquivalentProducts([FromUri] int productId) {
      try {
        base.RequireResource(productId, "productId");

        var product = Product.Parse(productId);

        FixedList<Product> equivalents = product.GetEquivalents();

        var array = new ArrayList(equivalents.Select((x) => PDMModels.GetProductModel(x)).ToArray());

        return new CollectionModel(this.Request, array, "Empiria.Trade.Product");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class EquivalentProductsController

}  // namespace Empiria.Trade.WebApi
