/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : StorageUnitKind                                  Pattern  : General Object Type               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Describes a storage unit kind.                                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Describes a warehousing operation.</summary>
  public class StorageUnitKind : GeneralObject {

    #region Constructors and parsers

    private StorageUnitKind() {
      // Required by Empiria Framework.
    }

    static public StorageUnitKind Empty {
      get { return BaseObject.ParseEmpty<StorageUnitKind>(); }
    }

    static public StorageUnitKind Parse(int id) {
      return BaseObject.ParseId<StorageUnitKind>(id);
    }

    static public FixedList<StorageUnitKind> GetList() {
      return GeneralObject.ParseList<StorageUnitKind>();
    }

    #endregion Constructors and parsers

  } // class StorageUnitKind

} // namespace Empiria.Trade.Ordering
