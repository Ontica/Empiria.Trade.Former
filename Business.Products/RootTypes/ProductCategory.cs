/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductCategory                                  Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a category of products used for product multiple classification or grouping.       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Products {

  /// <summary>Represents a category of products used for product multiple classification or grouping.</summary>
  public class ProductCategory : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.ProductCategory";

    #endregion Fields

    #region Constructors and parsers

    public ProductCategory()
      : base(thisTypeName) {

    }

    protected ProductCategory(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public ProductCategory Parse(int id) {
      return BaseObject.Parse<ProductCategory>(thisTypeName, id);
    }

    static public ProductCategory Empty {
      get { return BaseObject.ParseEmpty<ProductCategory>(thisTypeName); }
    }

    static public ProductCategory Unknown {
      get { return BaseObject.ParseUnknown<ProductCategory>(thisTypeName); }
    }

    #endregion Constructors and parsers

  } // class ProductCategory

} // namespace Empiria.Products
