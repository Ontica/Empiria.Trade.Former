/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductCategory                                  Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a category of products used for product multiple classification or grouping.       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    #endregion Constructors and parsers

  } // class ProductCategory

} // namespace Empiria.Products
