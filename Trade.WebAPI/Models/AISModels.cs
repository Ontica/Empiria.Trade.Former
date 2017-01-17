/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive.WebApi.Models               Assembly : Empiria.Trade.WebApi.dll            *
*  Type      : AISModels                                      Pattern  : Static class                        *
*  Version   : 2.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Static type that provides automotive industry object models for API consumption.              *
*                                                                                                            *
********************************* Copyright (c) 2009-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Automotive.WebApi.Models {

  /// <summary>Static type that provides automotive industry object models for API consumption.</summary>
  static internal class AISModels {

    #region Public methods

    static internal object GetEngineType(EngineType o) {
      return new {
        id = o.Id,
        asText = o.AsText,
      };
    }

    internal static object GetVehicleModel(VehicleModel o) {
      return new {
        id = o.Id,
        fromYear = o.FromYear,
        toYear = o.ToYear,
        make = o.Make.Name,
        model = o.Model,
        engine = o.Engine,
        asText = o.AsText,
        notes = o.Notes,
        description = o.Description
      };
    }

    #endregion Public methods

  }  // class AISModels

}  // namespace Empiria.Automotive.WebApi.Models
