/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Manufacturer                                     Pattern  : Storage Item                      *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Type that represents a product manufacturer.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  /// <summary> Type that represents a product manufacturer.</summary>
  public class Manufacturer : GeneralObject {

    #region Constructors and parsers

    private Manufacturer() {
      // Required by Empiria Framework.
    }

    static public Manufacturer Parse(int id) {
      return BaseObject.ParseId<Manufacturer>(id);
    }

    static public Manufacturer Empty {
      get { return BaseObject.ParseEmpty<Manufacturer>(); }
    }

    static public Labour Unknown {
      get { return BaseObject.ParseUnknown<Labour>(); }
    }

    static public FixedList<Manufacturer> GetList() {
      return GeneralObject.ParseList<Manufacturer>();
    }

    #endregion Constructors and parsers

  } // class Manufacturer

} // namespace Empiria.Products
