/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductTerm                                      Pattern  : Storage Item                      *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a product term.                                                                    *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

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

    static public ObjectList<ProductTerm> GetList() {
      ObjectList<ProductTerm> list = GeneralObject.ParseList<ProductTerm>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class ProductTerm

} // namespace Empiria.Products