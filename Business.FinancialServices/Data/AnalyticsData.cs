/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Financial Services                *
*  Namespace : Empiria.FinancialServices.Data                   Assembly : Empiria.FinancialServices.dll     *
*  Type      : AnalyticsData                                    Pattern  : Data Services Static Class        *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides data methods for financial services analytics.                                       *
*                                                                                                            *
********************************* Copyright (c) 2003-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.FinancialServices.Data {

  static public class AnalyticsData {

    #region Static fields

    #endregion Static fields

    #region Public methods

    static public DataTable AccountAnalysisByCycles(decimal unpaidCyclesBoundA, decimal unpaidCyclesBoundB,
                                            decimal unpaidCyclesBoundC, decimal creditBalanceLowerBound) {
      DataOperation operation = DataOperation.Parse("qryFSMAccountsAnalysis", unpaidCyclesBoundA, unpaidCyclesBoundB,
                                                    unpaidCyclesBoundC, creditBalanceLowerBound);
      return DataReader.GetDataTable(operation);
    }

    static public DataTable AccountAnalysisByDays(decimal unpaidDaysBoundA, decimal unpaidDaysBoundB,
                                                  decimal unpaidDaysBoundC, decimal creditBalanceLowerBound) {
      DataOperation operation = DataOperation.Parse("qryFSMAccountsAnalysisByDays", unpaidDaysBoundA, unpaidDaysBoundB,
                                                    unpaidDaysBoundC, creditBalanceLowerBound);
      return DataReader.GetDataTable(operation);
    }

    static public DataTable CollectorAnalysis(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAnalysisByCollector", fromDate, toDate.AddHours(23)));
    }

    static public DataTable ExecutiveAnalysis(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAnalysisByExecutive", fromDate, toDate.AddHours(23)));
    }

    static public DataTable ManagerAnalysis(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAnalysisByManager", fromDate, toDate.AddHours(23)));
    }

    static public DataTable SourceAnalysis(DateTime fromDate, DateTime toDate) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMAnalysisBySource", fromDate, toDate.AddHours(23)));
    }

    static public DataTable ManagerAnalysisByCycles(decimal unpaidCyclesBoundA, decimal unpaidCyclesBoundB,
                                           decimal unpaidCyclesBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMManagerAnalysis", unpaidCyclesBoundA, unpaidCyclesBoundB,
                                                          unpaidCyclesBoundC, creditBalanceLowerBound));
    }

    static public DataTable ManagerAnalysisByDays(decimal unpaidDaysBoundA, decimal unpaidDaysBoundB,
                                                  decimal unpaidDaysBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMManagerAnalysisByDays", unpaidDaysBoundA, unpaidDaysBoundB,
                                                         unpaidDaysBoundC, creditBalanceLowerBound));
    }

    static public DataTable SourceAnalysisByCycles(decimal unpaidCyclesBoundA, decimal unpaidCyclesBoundB,
                                           decimal unpaidCyclesBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMSourceAnalysis", unpaidCyclesBoundA, unpaidCyclesBoundB,
                                                          unpaidCyclesBoundC, creditBalanceLowerBound));
    }

    static public DataTable SourceAnalysisByCyclesAndCustomer(decimal unpaidCyclesBoundA, decimal unpaidCyclesBoundB,
                                           decimal unpaidCyclesBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMSourceAnalysisDDCustomer", 0, unpaidCyclesBoundA,
                                                         unpaidCyclesBoundB, unpaidCyclesBoundC, creditBalanceLowerBound));
    }

    static public DataTable SourceAnalysisByDays(decimal unpaidDaysBoundA, decimal unpaidDaysBoundB,
                                                 decimal unpaidDaysBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMSourceAnalysisByDays", unpaidDaysBoundA, unpaidDaysBoundB,
                                                         unpaidDaysBoundC, creditBalanceLowerBound));
    }

    static public DataTable SourceAnalysisByDaysAndCustomer(decimal unpaidDaysBoundA, decimal unpaidDaysBoundB,
                                                           decimal unpaidDaysBoundC, decimal creditBalanceLowerBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMSourceAnalysisByDaysDDCustomer", 0,
                                                         unpaidDaysBoundA, unpaidDaysBoundB, unpaidDaysBoundC, creditBalanceLowerBound));
    }

    static public DataTable CollectForecastBySource(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastBySource", startDate, periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectDataForecastBySource(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastBySource", startDate, periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectForecastByAccount(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastByAccount", startDate, periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectForecastForSourceByAccount(DateTime startDate, int organizationId, int periods, int periodLength,
                                                              decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastForSourceByAccount", startDate, organizationId,
                                                         periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectDataForecastByAccount(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastByAccount", startDate, periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectDataForecastForSourceByAccount(DateTime startDate, int organizationId, int periods, int periodLength,
                                                                  decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastForSourceByAccount", startDate, organizationId,
                                                         periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectForecastByManager(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastByManager", startDate, periodLength, unpayedCyclesBound));
    }

    static public DataTable CollectDataForecastByManager(DateTime startDate, int periods, int periodLength, decimal unpayedCyclesBound) {
      return DataReader.GetDataTable(DataOperation.Parse("qryFSMCollectForecastByManager", startDate, periodLength, unpayedCyclesBound));
    }

    #endregion Public methods

  } // class AnalyticsData

} // namespace Empiria.FinancialServices.Data
