/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Good                                             Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a physical good.                                                                   *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

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
