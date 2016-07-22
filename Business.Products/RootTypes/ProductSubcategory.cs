﻿/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductSubcategory                               Pattern  : Storage Item                      *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Second level of product aggrupation by product type or essence.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

namespace Empiria.Products {

  /// <summary>Second level of product aggrupation by product type or essence.</summary>
  public class ProductSubcategory : GeneralObject {

    #region Constructors and parsers

    private ProductSubcategory() {
      // Required by Empiria Framework.
    }

    static public ProductSubcategory Parse(int id) {
      return BaseObject.ParseId<ProductSubcategory>(id);
    }

    static public FixedList<ProductSubcategory> GetList() {
      return GeneralObject.ParseList<ProductSubcategory>();
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

    [Newtonsoft.Json.JsonIgnore]
    public ProductCategory Category {
      get {
        return base.GetInverseLink<ProductCategory>("Category-Subcategories");
      }
    }

    protected override void OnLoadObjectData(DataRow row) {
      if (!this.IsSpecialCase) {
        this.LegacyId = Convert.ToInt32((string) row["LegacyKey"]);
      }
    }

    public FixedList<ProductTerm> ProductTerms() {
      return base.GetLinks<ProductTerm>("Subcategory-ProductTerms");
    }

    #endregion Methods

  } // class ProductSubcategory

} // namespace Empiria.Products
