/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Manufacturer                                     Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Type that represents a product manufacturer.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Products {

  /// <summary> Type that represents a product manufacturer.</summary>
  public class Manufacturer : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.ProductManufacturer";

    #endregion Fields

    #region Constructors and parsers

    public Manufacturer()
      : base(thisTypeName) {

    }

    protected Manufacturer(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Manufacturer Parse(int id) {
      return BaseObject.Parse<Manufacturer>(thisTypeName, id);
    }

    static public Manufacturer Empty {
      get { return BaseObject.ParseEmpty<Manufacturer>(thisTypeName); }
    }

    static public Labour Unknown {
      get { return BaseObject.ParseUnknown<Labour>(thisTypeName); }
    }

    static public ObjectList<Manufacturer> GetList() {
      ObjectList<Manufacturer> list = GeneralObject.ParseList<Manufacturer>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class Manufacturer

} // namespace Empiria.Products