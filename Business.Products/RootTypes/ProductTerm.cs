/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductTerm                                      Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a product term.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    static public ProductTerm Unknown {
      get { return BaseObject.ParseUnknown<ProductTerm>(); }
    }

    static public FixedList<ProductTerm> GetList() {
      return GeneralObject.ParseList<ProductTerm>();
    }

    #endregion Constructors and parsers

  } // class ProductTerm

} // namespace Empiria.Products
