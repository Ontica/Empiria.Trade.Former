/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Trade Web API                     *
*  Namespace : Empiria.Trade.WebApi                             Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : ProductsController                               Pattern  : Web API                           *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Service to retrive and edit trading products information.                                     *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Linq;
using System.Web.Http;

using Empiria.Data;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Products;

namespace Empiria.Trade.WebApi {

  /// <summary>Service to retrive and edit trading products information.</summary>
  public class ProductsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/products")]
    public PagedCollectionModel GetProducts([FromUri] string searchFor) {
      try {
        base.RequireResource(searchFor, "searchFor");

        var expression = SearchExpression.ParseAndLike("ProductKeywords", searchFor);

        string sql = "SELECT * FROM PDMProducts WHERE " + expression.ToString() +
                     " ORDER BY ProductName, ProductCode";

        var data = DataReader.GetDataTable(DataOperation.Parse(sql));

        var list = Product.ParseList<Product>(data);

        var array = new System.Collections.ArrayList(list.Select((x) => GetProductModel(x)).ToArray());

        return new PagedCollectionModel(this.Request, array, "Empiria.Trade.Product");
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/products/{productId}")]
    public SingleObjectModel GetProductById([FromUri] int productId) {
      try {
        base.RequireResource(productId, "productId");

        var product = Product.Parse(productId);

        return new SingleObjectModel(this.Request, GetProductModel(product),
                                     "Empiria.Trade.Product");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private object GetProductModel(Product o, bool includeEquivalents = true) {
      return new {
        id = o.Id,
        productCode = o.ProductCode,
        brand = o.Brand,
        manufacturer = o.Manufacturer,
        productTerm = o.ProductTerm.Name,
        name = o.Name,
        description = o.Description,
        presentationUnit = o.PresentationUnit.Name,
        baseProductId = o.BaseProduct.Id
      };
    }

    #endregion Private methods

  }  // class ProductsController

}  // namespace Empiria.Trade.WebApi
