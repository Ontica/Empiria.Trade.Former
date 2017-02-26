/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Trade.WebApi                           Assembly : Empiria.Trade.WebApi.dll            *
*  Type      : VehicleModelSearchController                   Pattern  : Web API                             *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Web API that provides services to get vehicle model information.                              *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections;
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

        var array = new ArrayList(makes.Select((x) => x.Name).ToArray());

        return new CollectionModel(this.Request, array, "Empiria.Automotive.Make");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/years/{year}/makes/{makeName}/models")]
    public CollectionModel GetYearMakeModels(int year, string makeName) {
      try {
        base.RequireResource(year, "year");
        base.RequireResource(makeName, "makeName");

        makeName = EmpiriaString.DecodeUrlIdentifier(makeName);

        var make = Make.Parse(makeName);

        var models = VehicleModelSearcher.GetModelsInYear(year, make);

        return new CollectionModel(this.Request, models, "Empiria.Automotive.Model");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/automotive/years/{year}/makes/{makeName}/models/{modelName}/engines")]
    public CollectionModel GetYearMakeModelEngines(int year, string makeName, string modelName) {
      try {
        base.RequireResource(year, "year");
        base.RequireResource(makeName, "makeName");
        base.RequireResource(modelName, "modelName");

        makeName = EmpiriaString.DecodeUrlIdentifier(makeName);
        modelName = EmpiriaString.DecodeUrlIdentifier(modelName);

        var make = Make.Parse(makeName);

        var engines = VehicleModelSearcher.GetModelEnginesInYear(year, make, modelName);

        var array = new ArrayList(engines.Select((x) => AISModels.GetEngineType(x)).ToArray());

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

        return new SingleObjectModel(this.Request,
                                     AISModels.GetVehicleModel(vehicleModel),
                                    "Empiria.Automotive.VehicleModel");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class VehicleModelSearchController

}  // namespace Empiria.Automotive.WebApi
