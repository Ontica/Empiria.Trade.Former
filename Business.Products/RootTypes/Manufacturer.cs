/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Manufacturer                                     Pattern  : Storage Item                      *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Type that represents a product manufacturer.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
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


    #region Constructors and parsers

    public new string Name {
      get {
        return base.Name;
      }
    }

    #endregion Constructors and parsers


  } // class Manufacturer

} // namespace Empiria.Products
