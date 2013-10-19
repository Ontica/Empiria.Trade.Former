/* Empiria® Trade 2013 ***************************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Trade                                   System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : StorageUnitKind                                  Pattern  : General Object Type               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Describes a storage unit kind.                                                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
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

    static public ObjectList<StorageUnitKind> GetList() {
      ObjectList<StorageUnitKind> list = GeneralObject.ParseList<StorageUnitKind>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class StorageUnitKind

} // namespace Empiria.Trade.Ordering