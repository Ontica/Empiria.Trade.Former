/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Trade.WebApi                           Assembly : Empiria.Trade.WebApi.dll            *
*  Type      : VehicleModelSearchController                   Pattern  : Web API                             *
*  Version   : 2.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Web API that provides services to get vehicle model information.                              *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Linq;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Automotive.WebApi.Models;

namespace Empiria.Automotive.WebApi {

  /// <summary>Web API that provides services to get vehicle model information.</summary>
  public class VehicleModelSearchController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/automotive/years")]
    public CollectionModel GetYears() {
      try {

        var years = VehicleModelSearcher.GetYears();

        years.Reverse();

        return new CollectionModel(this.Request, years);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/years/{year}/makes")]
    public CollectionModel GetYearMakes(int year) {
      try {
        base.RequireResource(year, "year");

        var makes = VehicleModelSearcher.GetMakesInYear(year);

        var array = new System.Collections.ArrayList(makes.Select((x) => AISModels.GetMake(x)).ToArray());

        return new CollectionModel(this.Request, array, "Empiria.Automotive.Make");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/years/{year}/makes/{makeKey}/models")]
    public CollectionModel GetYearMakeModels(int year, string makeKey) {
      try {
        base.RequireResource(year, "year");
        base.RequireResource(makeKey, "makeKey");

        var make = Make.Parse(makeKey);

        var models = VehicleModelSearcher.GetModelsInYear(year, make);

        return new CollectionModel(this.Request, models);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/years/{year}/makes/{makeKey}/models/{modelName}/engines")]
    public CollectionModel GetYearMakeModelEngines(int year, string makeKey, string modelName) {
      try {
        base.RequireResource(year, "year");
        base.RequireResource(makeKey, "makeKey");
        base.RequireResource(modelName, "modelName");

        var make = Make.Parse(makeKey);

        var engines = VehicleModelSearcher.GetModelEnginesInYear(year, make, modelName);

        var array = new System.Collections.ArrayList(engines.Select((x) => AISModels.GetEngineType(x)).ToArray());

        return new CollectionModel(this.Request, array, "Empiria.Automotive.EngineType");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/vehicle-models/{vehicleModelId}")]
    public SingleObjectModel GetVehicleModel(int vehicleModelId) {
      try {
        base.RequireResource(vehicleModelId, "vehicleModelId");

        var vehicleModel = VehicleModel.Parse(vehicleModelId);

        return new SingleObjectModel(this.Request, vehicleModel);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    //[HttpGet]
    //[Route("v1/automotive/years/{year}/makes/{makeId}/models/{model}/trim-levels")]
    //public CollectionModel GetYearMakeModelTrimLevels(int year, int makeId, string model) {
    //  try {
    //    base.RequireResource(year, "year");
    //    base.RequireResource(makeId, "makeId");
    //    base.RequireResource(model, "model");

    //    var make = Make.Parse(makeId);

    //    FixedList<string> trimLevels = make.GetTrimLevels(year, model);

    //    return new CollectionModel(this.Request, trimLevels);

    //  } catch (Exception e) {
    //      throw base.CreateHttpException(e);
    //  }
    //}


    //[HttpGet]
    //[Route("v1/automotive/years/{year}/makes/{makeId}/models/{model}/trim-levels/{trimLevel}/engines")]
    //public CollectionModel GetYearMakeModelTrimLevelEngines(int year, int makeId,
    //                                                        string model, string trimLevel) {
    //  try {
    //    base.RequireResource(year, "year");
    //    base.RequireResource(makeId, "makeId");
    //    base.RequireResource(model, "model");
    //    base.RequireResource(trimLevel, "trimLevel");

    //    var make = Make.Parse(makeId);

    //    FixedList<string> engines = make.GetEngines(year, model, trimLevel);

    //    return new CollectionModel(this.Request, engines);

    //  } catch (Exception e) {
    //    throw base.CreateHttpException(e);
    //  }
    //}


    #endregion Public APIs

  }  // class VehicleModelSearchController

}  // namespace Empiria.Automotive.WebApi
