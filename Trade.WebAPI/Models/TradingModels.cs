/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive.WebApi.Models               Assembly : Empiria.Trade.WebApi.dll            *
*  Type      : TradingModels                                  Pattern  : Static class                        *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Static type that provides trading object models for API consumption.                          *
*                                                                                                            *
********************************* Copyright (c) 2009-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Contacts;

namespace Empiria.Trade.WebApi.Models {

  /// <summary>Static type that provides trading models for API consumption.</summary>
  static internal class TradingModels {

    #region Public methods

    static internal object GetTrader(Contact o) {
      return new {
        id = o.Id,
        fullName = o.FullName,
        nickName = o.Nickname
      };
    }

    #endregion Public methods

  }  // class TradingModels

}  // namespace Empiria.Trade.WebApi.Models
