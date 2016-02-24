using System;
using System.Linq;
using System.Web.Http;

using Empiria.Data;
using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Products;

namespace Empiria.Trade.WebApi {

  public class ProductsController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/products")]
    public PagedCollectionModel GetProducts([FromUri] string searchFor) {
      try {
        base.RequireResource(searchFor, "searchFor");

        var expression = SearchExpression.ParseAndLike("ProductKeywords", searchFor);

        string sql = "SELECT * FROM PDMProducts WHERE " + expression.ToString() +
                     " ORDER BY ProductName, PartNumber";

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

    [HttpPost]
    [Route("v1/products/{baseProductId}/equivalents/{equivalentProductId}")]
    public SingleObjectModel AddEquivalent([FromUri] int baseProductId,
                                           [FromUri] int equivalentProductId) {
      try {
        base.RequireResource(baseProductId, "baseProductId");
        base.RequireResource(equivalentProductId, "equivalentProductId");

        var baseProduct = Product.Parse(baseProductId);
        var equivalentProduct = Product.Parse(equivalentProductId);

        baseProduct.AddEquivalent(equivalentProduct);

        return new SingleObjectModel(this.Request, GetProductModel(baseProduct),
                                     "Empiria.Trade.Product");
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpDelete]
    [Route("v1/products/{baseProductId}/equivalents/{equivalentProductId}")]
    public SingleObjectModel DeleteEquivalent([FromUri] int baseProductId,
                                              [FromUri] int equivalentProductId) {
      try {
        base.RequireResource(baseProductId, "baseProductId");
        base.RequireResource(equivalentProductId, "equivalentProductId");

        var baseProduct = Product.Parse(baseProductId);
        var equivalentProduct = Product.Parse(equivalentProductId);

        baseProduct.RemoveEquivalent(equivalentProduct);

        return new SingleObjectModel(this.Request, GetProductModel(baseProduct),
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
        partNumber = o.PartNumber,
        brand = o.Brand.Name,
        brandId = o.Brand.Id,
        brandLegacyId = o.Brand.LegacyId,
        productTerm = o.ProductTerm.Name,
        name = o.Name,
        specification = o.Specification,
        presentationUnit = o.PresentationUnit.Name,
        equivalents = includeEquivalents ? o.Equivalents.Select((x) => GetProductModel(x))
                                         : new Array[0]
      };
    }

    #endregion Private methods

  }  // class PropertyController

}  // namespace Empiria.Land.WebApi
