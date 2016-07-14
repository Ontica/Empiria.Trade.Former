/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductCategory                                  Pattern  : Storage Item                      *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a category of products used for product multiple classification or grouping.       *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  /// <summary>Represents a category of products used for product multiple classification or grouping.</summary>
  public class ProductCategory : GeneralObject {

    #region Constructors and parsers

    private ProductCategory() {
      // Required by Empiria Framework.
    }

    static public ProductCategory Parse(int id) {
      return BaseObject.ParseId<ProductCategory>(id);
    }

    static public ProductCategory Empty {
      get { return BaseObject.ParseEmpty<ProductCategory>(); }
    }

    static public ProductCategory Unknown {
      get { return BaseObject.ParseUnknown<ProductCategory>(); }
    }

    public static FixedList<ProductCategory> GetList() {
      return GeneralObject.ParseList<ProductCategory>();
    }

    #endregion Constructors and parsers


    #region Methods

    public FixedList<ProductCategory> Subcategories() {
      throw new NotImplementedException();
    }

    #endregion Methods

  } // class ProductCategory

} // namespace Empiria.Products
