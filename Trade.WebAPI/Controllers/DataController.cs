using System;
using System.Data;
using System.Web.Http;

using Empiria.Data.Convertion;
using Empiria.WebApi;
using Empiria.WebApi.Models;

namespace Empiria.Trade.WebApi {

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

}  // namespace Empiria.Land.WebApi
