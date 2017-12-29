/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductTerm                                      Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a product term.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Products {

  /// <summary>Represents a product term.</summary>
  public class ProductTerm : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.ProductTerm";

    #endregion Fields

    #region Constructors and parsers

    public ProductTerm()
      : base(thisTypeName) {

    }

    protected ProductTerm(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public ProductTerm Parse(int id) {
      return BaseObject.Parse<ProductTerm>(thisTypeName, id);
    }

    static public ProductTerm Empty {
      get { return BaseObject.ParseEmpty<ProductTerm>(thisTypeName); }
    }

    static public ProductTerm Unknown {
      get { return BaseObject.ParseUnknown<ProductTerm>(thisTypeName); }
    }

    static public FixedList<ProductTerm> GetList() {
      FixedList<ProductTerm> list = GeneralObject.ParseList<ProductTerm>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public string TaxAuthCode {
      get {
        return base.Value;
      }
    }

    #endregion Properties

  } // class ProductTerm

} // namespace Empiria.Products
