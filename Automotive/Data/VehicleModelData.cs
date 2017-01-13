/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive.Data                        Assembly : Empiria.Automotive.dll              *
*  Type      : VehicleModelData                               Pattern  : Data Services Static Class          *
*  Version   : 2.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Provides data read and write methods for vehicle version data.                                *
*                                                                                                            *
********************************* Copyright (c) 2008-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;

using System.Data;

using Empiria.Data;

namespace Empiria.Automotive.Data {

  /// <summary>Provides data read methods for retail store data.</summary>
  static internal class VehicleModelData {

    #region Public methods

    static internal FixedList<string> GetEnginesInYear(int year, Make make, string model) {
      var operation = DataOperation.Parse("qryAISModelEnginesInYear", year, make.Id, model);

      List<string> models = DataReader.GetFieldValues<string>(operation);

      return models.ToFixedList();
    }


    static internal FixedList<Make> GetMakesInYear(int year) {
      var operation = DataOperation.Parse("qryAISMakesInYear", year);

      List <Make> makes = DataReader.GetList<Make>(operation, (x) => BaseObject.ParseList<Make>(x));

      return makes.ToFixedList();
    }

    internal static FixedList<EngineType> GetModelEnginesInYear(int year, Make make, string modelName) {
      var operation = DataOperation.Parse("qryAISModelEnginesInYear", year, make.Id, modelName);

      List<EngineType> engine = DataReader.GetList<EngineType>(operation, (x) => BaseObject.ParseList<EngineType>(x));

      return engine.ToFixedList();
    }

    static internal FixedList<string> GetModelsInYear(int year, Make make) {
      var operation = DataOperation.Parse("qryAISModelsInYear", year, make.Id);

      List<string> models = DataReader.GetFieldValues<string>(operation);

      return models.ToFixedList();
    }


    static internal FixedList<int> GetYears() {
      var operation = DataOperation.Parse("qryAISModelYearsRange");

      DataRow minMaxYears = DataReader.GetDataRow(operation);

      int minYear = (int) minMaxYears["MinYear"];
      int maxYear = (int) minMaxYears["MaxYear"];

      var list = new List<int>(maxYear - minYear + 1);

      for (int i = minYear; i <= maxYear; i++) {
        list.Add(i);
      }

      return list.ToFixedList();
    }

    #endregion Public methods

  } // class VehicleModelData

} // namespace Empiria.Automotive.Data
