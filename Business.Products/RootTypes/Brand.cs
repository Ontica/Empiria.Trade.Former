/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Brand                                            Pattern  : Storage Item                      *
*  Version   : 2.0                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes a product brand.                                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  /// <summary>Describes a product brand.</summary>
  public class Brand : GeneralObject {

    #region Constructors and parsers

    private Brand() {
      // Required by Empiria Framework.
    }

    static public Brand Parse(int id) {
      return BaseObject.ParseId<Brand>(id);
    }

    static public Brand Empty {
      get { return BaseObject.ParseEmpty<Brand>(); }
    }

    static public Brand Unknown {
      get { return BaseObject.ParseUnknown<Brand>(); }
    }

    static public FixedList<Brand> GetList() {
      return GeneralObject.ParseList<Brand>();
    }

    #endregion Constructors and parsers

    #region Fields

    [DataField(GeneralObject.ExtensionDataFieldName + ".LegacyId")]
    public int LegacyId {
      get;
      private set;
    }

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Fields

  } // class Brand

} // namespace Empiria.Products
