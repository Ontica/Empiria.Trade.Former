/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductTerm                                      Pattern  : Storage Item                      *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a product term.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  /// <summary>Represents a product term.</summary>
  public class ProductTerm : GeneralObject {

    #region Constructors and parsers

    private ProductTerm() {
      // Required by Empiria Framework.
    }

    static public ProductTerm Parse(int id) {
      return BaseObject.ParseId<ProductTerm>(id);
    }

    static public ProductTerm Empty {
      get { return BaseObject.ParseEmpty<ProductTerm>(); }
    }

    static public FixedList<ProductTerm> GetList(string searchFor) {
      var filter = SearchExpression.ParseAndLike("ObjectKeywords", searchFor);

      return GeneralObject.ParseList<ProductTerm>(filter);
    }

    #endregion Constructors and parsers


    #region Properties

    public new string Name {
      get {
        return base.Name;
      }
    }

    [Newtonsoft.Json.JsonIgnore]
    public ProductCategory Category {
      get {
        return this.Subcategory.Category;
      }
    }

    Lazy<ProductSubcategory> _subcategory = new Lazy<ProductSubcategory>();
    [Newtonsoft.Json.JsonIgnore]
    public ProductSubcategory Subcategory {
      get {
        return _subcategory.Value;
      }
    }


    #endregion Properties

    #region Methods

    protected override void OnInitialize() {
      base.OnInitialize();
      _subcategory = new Lazy<ProductSubcategory>(() => base.GetInverseLink<ProductSubcategory>("Subcategory-ProductTerms"));
    }

    #endregion Methods

  } // class ProductTerm

} // namespace Empiria.Products
