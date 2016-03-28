﻿/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : DeliveryMode                                     Pattern  : General Object Type               *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a delivery type or condition like not delivery, store, pick, air, land.            *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Represents a delivery type or condition like not delivery, store, pick, air, land.</summary>
  public class DeliveryMode : GeneralObject {

    #region Constructors and parsers

    private DeliveryMode() {
      // Required by Empiria Framework.
    }

    static public DeliveryMode Empty {
      get { return BaseObject.ParseEmpty<DeliveryMode>(); }
    }

    static public DeliveryMode Parse(int id) {
      return BaseObject.ParseId<DeliveryMode>(id);
    }

    static public FixedList<DeliveryMode> GetList() {
      return GeneralObject.ParseList<DeliveryMode>();
    }

    #endregion Constructors and parsers

    #region Properties

    public bool IsNoDeliveryMode {
      get { return (base.NamedKey == "N"); }
    }

    public bool IsCargoDeliveryMode {
      get { return (base.NamedKey == "E"); }
    }

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class DeliveryMode

} // namespace Empiria.Trade.Ordering
