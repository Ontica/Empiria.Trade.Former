/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management API       *
*  Namespace : Empiria.Trade.PDM.WebApi                         Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : CataloguesController                             Pattern  : Web API                           *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Services to read brands, product types, units, and other product related information.         *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
using System.Linq;
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
    public CollectionModel GetBrandList([FromUri] string searchFor = "") {
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
    public CollectionModel GetManufacturerList([FromUri] string searchFor = "") {
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
    public CollectionModel GetProductCategoriesList() {
      try {
        var list = ProductCategory.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/product-categories/{categoryId}")]
    public SingleObjectModel GetProductCategory([FromUri] int categoryId) {
      try {
        var category = ProductCategory.Parse(categoryId);

        return new SingleObjectModel(this.Request, category);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/product-categories/{categoryId}/sub-categories")]
    public CollectionModel GetProductSubcategoriesList([FromUri] int categoryId) {
      try {
        var category = ProductCategory.Parse(categoryId);

        var list = category.Subcategories();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/product-data/product-sub-categories/{subcategoryId}")]
    public SingleObjectModel GetProductSubcategory([FromUri] int subcategoryId) {
      try {
        var subcategory = ProductSubcategory.Parse(subcategoryId);

        return new SingleObjectModel(this.Request, subcategory);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/product-data/product-sub-categories/{subcategoryId}/product-terms")]
    public CollectionModel GetSubcategoryProductTermsList([FromUri] int subcategoryId) {
      try {
        var subcategory = ProductSubcategory.Parse(subcategoryId);

        var list = subcategory.ProductTerms();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/product-terms")]
    public CollectionModel GetProductTermsList([FromUri] string searchFor = "") {
      try {
        var list = ProductTerm.GetList(searchFor);

        return new CollectionModel(this.Request, this.ToProductTermModel(list), typeof(ProductTerm).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpGet]
    [Route("v1/product-data/product-terms/{productTermId}")]
    public SingleObjectModel GetProductTerm([FromUri] int productTermId) {
      try {
        var productTerm = ProductTerm.Parse(productTermId);

        return new SingleObjectModel(this.Request, productTerm);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    private ArrayList ToProductTermModel(FixedList<ProductTerm> list) {
      return new ArrayList(list.Select((x) => ToProductTermModel(x)).ToArray());
    }

    private object ToProductTermModel(ProductTerm o) {
      return new {
        id = o.Id,
        name = o.Name,
        subcategoryId = o.Subcategory.Id,
        categoryId = o.Category.Id,
      };
    }

    #endregion Product classification


    #region Presentation and Units


    [HttpGet]
    [Route("v1/product-data/presentation-units")]
    public CollectionModel GetPresentationUnitsList() {
      try {
        var list = PresentationUnit.GetList();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/presentation-units/{presentationUnitId}")]
    public SingleObjectModel GetPresentationUnit([FromUri] int presentationUnitId) {
      try {
        var presentationUnit = PresentationUnit.Parse(presentationUnitId);

        return new SingleObjectModel(this.Request, presentationUnit);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/presentation-units/{presentationUnitId}/content-units")]
    public CollectionModel GetPresentationContentsUnitsList([FromUri] int presentationUnitId) {
      try {
        var presentationUnit = PresentationUnit.Parse(presentationUnitId);

        var list = presentationUnit.ContentsUnits();

        return new CollectionModel(this.Request, list);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/product-data/content-units/{contentUnitId}")]
    public SingleObjectModel GetContentUnit([FromUri] int contentUnitId) {
      try {
        var contentUnit = Empiria.DataTypes.Unit.Parse(contentUnitId);

        return new SingleObjectModel(this.Request, contentUnit);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Presentation and Units


    #region Product Manager

    //[HttpGet]
    //[Route("v1/product-data/product-managers")]
    //public CollectionModel GetProductManagerList() {
    //  try {
    //    var list = Manufacturer.GetList();

    //    return new CollectionModel(this.Request, list);
    //  } catch (Exception e) {
    //    throw base.CreateHttpException(e);
    //  }
    //}


    //[HttpGet]
    //[Route("v1/product-data/product-managers/{productManagerId}")]
    //public SingleObjectModel GetProductManager([FromUri] int productManagerId) {
    //  try {
    //    var productManager = Manufacturer.Parse(productManagerId);

    //    return new SingleObjectModel(this.Request, productManager);
    //  } catch (Exception e) {
    //    throw base.CreateHttpException(e);
    //  }
    //}

    #endregion Product Manager

  }  // class CataloguesController

}  // namespace Empiria.Trade.PDM.WebApi
