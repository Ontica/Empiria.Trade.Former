/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Brand                                            Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Describes a product brand.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Products {

  /// <summary>Describes a product brand.</summary>
  public class Brand : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.ProductBrand";

    #endregion Fields

    #region Constructors and parsers

    public Brand()
      : base(thisTypeName) {

    }

    protected Brand(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Brand Parse(int id) {
      return BaseObject.Parse<Brand>(thisTypeName, id);
    }

    static public Brand Empty {
      get { return BaseObject.ParseEmpty<Brand>(thisTypeName); }
    }

    static public Brand Unknown {
      get { return BaseObject.ParseUnknown<Brand>(thisTypeName); }
    }

    static public ObjectList<Brand> GetList() {
      ObjectList<Brand> list = GeneralObject.ParseList<Brand>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Fields

    public int LegacyId {
      get { return int.Parse(base.Description); }
    }

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    #endregion Fields

  } // class Brand

} // namespace Empiria.Products