/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Good                                             Pattern  : Storage Item                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a physical good.                                                                   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.Products {

  public sealed class Good : Product {

    #region Fields

    private const string thisTypeName = "ObjectType.Product.Good";

    #endregion Fields

    #region Constructors and parsers

    public Good()
      : base(thisTypeName) {

    }

    private Good(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public new Good Parse(int id) {
      return BaseObject.Parse<Good>(thisTypeName, id);
    }

    #endregion Constructors and parsers

  } // class Good

} // namespace Empiria.Products