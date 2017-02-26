/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductCategory                                  Pattern  : Storage Item                      *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : First level of product aggrupation by product type or essence.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Products {

  /// <summary>First level of product aggrupation by product type or essence.</summary>
  public class ProductCategory : GeneralObject {

    #region Constructors and parsers

    private ProductCategory() {
      // Required by Empiria Framework.
    }

    static public ProductCategory Parse(int id) {
      return BaseObject.ParseId<ProductCategory>(id);
    }

    static public FixedList<ProductCategory> GetList() {
      return GeneralObject.ParseList<ProductCategory>();
    }

    #endregion Constructors and parsers

    #region Fields


    public new string Name {
      get {
        return base.Name;
      }
    }

    [Newtonsoft.Json.JsonIgnore]
    public int LegacyId {
      get;
      private set;
    }

    #endregion Fields

    #region Methods

    protected override void OnLoadObjectData(DataRow row) {
      if (!this.IsSpecialCase) {
        this.LegacyId = Convert.ToInt32((string) row["LegacyKey"]);
      }
    }

    public FixedList<ProductSubcategory> Subcategories() {
      return base.GetLinks<ProductSubcategory>("Category-Subcategories");
    }

    #endregion Methods

  } // class ProductCategory

} // namespace Empiria.Products
