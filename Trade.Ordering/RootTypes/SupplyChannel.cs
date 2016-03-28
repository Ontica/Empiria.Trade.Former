/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyChannel                                    Pattern  : General Object Type               *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Describes a supply channel like Internet, telephone, catalogue or store.                      *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Describes a supply channel like Internet, telephone, catalogue or store.</summary>
  public class SupplyChannel : GeneralObject {

    #region Constructors and parsers

    private SupplyChannel() {
      // Required by Empiria Framework.
    }

    static public SupplyChannel Empty {
      get { return BaseObject.ParseEmpty<SupplyChannel>(); }
    }

    static public SupplyChannel Parse(int id) {
      return BaseObject.ParseId<SupplyChannel>(id);
    }

    static public FixedList<SupplyChannel> GetList() {
      return GeneralObject.ParseList<SupplyChannel>();
    }

    #endregion Constructors and parsers

  } // class SupplyChannel

} // namespace Empiria.Trade.Ordering
