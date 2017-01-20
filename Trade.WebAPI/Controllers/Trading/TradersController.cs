/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Trade Web API                     *
*  Namespace : Empiria.Trade.WebApi                             Assembly : Empiria.Trade.WebApi.dll          *
*  Type      : TradersController                                Pattern  : Web API                           *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Service to retrive and manage traders of goods and services.                                  *
*                                                                                                            *
********************************* Copyright (c) 2014-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Linq;
using System.Web.Http;

using Empiria.WebApi;
using Empiria.WebApi.Models;

using Empiria.Trade.Data;
using Empiria.Trade.WebApi.Models;

namespace Empiria.Trade.WebApi {

  /// <summary>Service to retrive and edit trading products information.</summary>
  public class TradersController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/traders")]
    public PagedCollectionModel GetTraders([FromUri] string searchFor = "") {
      try {
        // base.RequireResource(searchFor, "searchFor");

        // var expression = SearchExpression.ParseAndLike("SupplierKeywords", searchFor);

        var list = SupplyOrdersData.GetSuppliers();

        var array = new System.Collections.ArrayList(list.Select((x) => TradingModels.GetTrader(x)).ToArray());

        return new PagedCollectionModel(this.Request, array, "Empiria.Trade.Trader");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class TradersController

}  // namespace Empiria.Trade.WebApi
