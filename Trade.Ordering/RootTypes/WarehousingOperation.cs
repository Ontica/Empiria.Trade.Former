/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : WarehousingOperation                             Pattern  : General Object Type               *
*  Version   : 2.0                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes a warehousing operation.                                                            *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Describes a warehousing operation.</summary>
  public class WarehousingOperation : GeneralObject {

    #region Constructors and parsers

    private WarehousingOperation() {
      // Required by Empiria Framework.
    }

    static public WarehousingOperation Empty {
      get { return BaseObject.ParseEmpty<WarehousingOperation>(); }
    }

    static public WarehousingOperation Parse(int id) {
      return BaseObject.ParseId<WarehousingOperation>(id);
    }

    static public FixedList<WarehousingOperation> GetList() {
      return GeneralObject.ParseList<WarehousingOperation>();
    }

    public FixedList<WarehousingOperation> GetDocumentTypes() {
      var list = this.GetLinks<WarehousingOperation>("TransactionType_DocumentType");

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class WarehousingOperation

} // namespace Empiria.Trade.Ordering
