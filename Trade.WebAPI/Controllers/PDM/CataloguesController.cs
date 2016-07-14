/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management API       *
*  Namespace : Empiria.Trade.PDM.WebApi                         Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : CataloguesController                             Pattern  : Web API                           *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Services to read brands, product types, units, and other product related information.         *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Products;

namespace Empiria.Trade.PDM.WebApi {

  /// <summary>Services to read brands, product types, units, and other product related information.</summary>
  public class CataloguesController : WebApiController {

    #region Manufacturers and Brands

    [HttpGet]
    [Route("v1/product-data/brands")]
    public CollectionModel GetBrandList([FromUri] string search = "") {
      try {
        var list = Brand.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/brands/{brandId}")]
    public SingleObjectModel GetBrand([FromUri] int brandId) {
      try {
        var brand = Brand.Parse(brandId);

        return new SingleObjectModel(this.Request, brand);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/manufacturers")]
    public CollectionModel GetManufacturerList([FromUri] string search = "") {
      try {
        var list = Manufacturer.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/manufacturers/{manufacturerId}")]
    public SingleObjectModel GetManufacturer([FromUri] int manufacturerId) {
      try {
        var manufacturer = Manufacturer.Parse(manufacturerId);

        return new SingleObjectModel(this.Request, manufacturer);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Manufacturers and Brands


    #region Product classification

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


    [HttpGet]
    [Route("v1/product-data/product-categories/{categoryId}/sub-categories")]
    public CollectionModel GetProductSubCategories([FromUri] int categoryId, [FromUri] string search = "") {
      try {
        var category = ProductCategory.Parse(categoryId);

        var list = category.Subcategories();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


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

    #endregion Product classification


    #region Presentation and Units


    [HttpGet]
    [Route("v1/product-data/presentation-units/{presentationUnitId}/content-units")]
    public CollectionModel GetContentsUnits([FromUri] int presentationUnitId) {
      try {
        var presentationUnit = PresentationUnit.Parse(presentationUnitId);

        var list = presentationUnit.GetContentsUnits();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/presentation-units")]
    public CollectionModel GetPresentationUnits() {
      try {
        var list = PresentationUnit.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    #endregion Presentation and Units

  }  // class CataloguesController

}  // namespace Empiria.Trade.PDM.WebApi
