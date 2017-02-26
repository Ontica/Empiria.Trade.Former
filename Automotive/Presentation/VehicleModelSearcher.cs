/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive                             Assembly : Empiria.Automotive.dll              *
*  Type      : VehicleModelSearcher                           Pattern  : Static Class                        *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Provides search services for vehicle models.                                                  *
*                                                                                                            *
********************************* Copyright (c) 2008-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Automotive.Data;

namespace Empiria.Automotive {

  /// <summary>Provides search services for vehicle models.</summary>
  static public class VehicleModelSearcher {

    #region Public methods

    /// <summary>Gets an array with the range of years involved in
    /// all registered vehicle versions.</summary>
    /// <returns>An integer array with the range of vehicle years.</returns>
    static public FixedList<int> GetYears() {
      return VehicleModelData.GetYears();
    }

    static public FixedList<Make> GetMakesInYear(int year) {
      return VehicleModelData.GetMakesInYear(year);
    }

    static public FixedList<EngineType> GetModelEnginesInYear(int year, Make make, string modelName) {
      return VehicleModelData.GetModelEnginesInYear(year, make, modelName);
    }

    static public FixedList<string> GetModelsInYear(int year, Make make) {
      return VehicleModelData.GetModelsInYear(year, make);
    }

    #endregion Public methods

  } // class VehicleModelSearcher

} // namespace Empiria.Automotive
