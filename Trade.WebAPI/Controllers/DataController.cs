/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Trade Web API                     *
*  Namespace : Empiria.Trade.WebApi                             Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : DataController                                   Pattern  : Web API                           *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provisional service to get and set data stored in customer MySQL legacy database.             *
*                                                                                                            *
********************************* Copyright (c) 2014-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;
using System.Web.Http;

using Empiria.Data.Convertion;
using Empiria.WebApi;

namespace Empiria.Trade.WebApi {

  /// <summary>Provisional service to get and set data stored in customer MySQL legacy database.</summary>
  public class DataController : WebApiController {

    #region Public APIs

    [HttpPost]
    [Route("v1/mysql/get-data")]
    public PagedCollectionModel GetData([FromBody] object sql) {
      try {
        var sqlText = Empiria.Json.JsonObject.Parse(sql).Get<string>("sql");

        DataConvertionEngine converter = DataConvertionEngine.GetInstance();
        converter.Initalize("Empiria", "Autopartes.MySQL");

        DataTable table = converter.GetTargetDataTable(sqlText, "Data");

        return new PagedCollectionModel(this.Request, table);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    [HttpPost]
    [Route("v1/mysql/set-data")]
    public SingleObjectModel SetData([FromBody] object sql) {
      try {
        var sqlText = Empiria.Json.JsonObject.Parse(sql).Get<string>("sql");

        DataConvertionEngine converter = DataConvertionEngine.GetInstance();
        converter.Initalize("Empiria", "Autopartes.MySQL");

        int result = converter.Execute(new string[] { sqlText });

        return new SingleObjectModel(this.Request, result);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class PropertyController

}  // namespace Empiria.Trade.WebApi
