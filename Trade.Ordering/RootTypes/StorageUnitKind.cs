/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : StorageUnitKind                                  Pattern  : General Object Type               *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Describes a storage unit kind.                                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Describes a warehousing operation.</summary>
  public class StorageUnitKind : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.Trade.StorageUnitKind";

    #endregion Fields

    #region Constructors and parsers

    public StorageUnitKind()
      : base(thisTypeName) {

    }

    protected StorageUnitKind(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public StorageUnitKind Empty {
      get { return BaseObject.ParseEmpty<StorageUnitKind>(thisTypeName); }
    }

    static public StorageUnitKind Parse(int id) {
      return BaseObject.Parse<StorageUnitKind>(thisTypeName, id);
    }

    static public FixedList<StorageUnitKind> GetList() {
      FixedList<StorageUnitKind> list = GeneralObject.ParseList<StorageUnitKind>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class StorageUnitKind

} // namespace Empiria.Trade.Ordering
