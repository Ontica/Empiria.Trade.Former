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
    [Route("v1/product-data/products")]
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
    [Route("v1/product-data/products/{productId}")]
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

    [HttpPost]
    [Route("v1/product-data/products/{productId}")]
    public SingleObjectModel UpdateProduct([FromUri] int productId, [FromBody] Product changes) {
      try {
        base.RequireResource(productId, "productId");
        base.RequireBody(changes);

        var product = Product.Parse(productId);
        //product.Update(changes);

        return new SingleObjectModel(this.Request, GetProductModel(product), "Empiria.Trade.Product");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

    #region Private methods

    private object GetProductModel(Product o, bool includeEquivalents = true) {
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

    #endregion Private methods

  }  // class ProductsController

}  // namespace Empiria.Trade.WebApi
