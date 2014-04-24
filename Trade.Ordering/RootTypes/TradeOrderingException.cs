/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : TradeOrderingException                           Pattern  : Empiria Exception Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in Empiria Trade Ordering System.          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Reflection;

namespace Empiria.Trade.Ordering {

  /// <summary>The exception that is thrown when a problem occurs in Empiria Trade Ordering System.</summary>
  [Serializable]
  public sealed class TradeOrderingException : EmpiriaException {

    public enum Msg {
      CancelBillStamperException,
      CreateBillStamperException,
      InvalidCategoryRule,
      InvalidChildCategory,
      InvalidPeriodForDailyBills,
      UnrecognizedBillType,
    }

    static private string resourceBaseName = "Empiria.Trade.Ordering.RootTypes.TradeOrderingExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of OrderingSystemException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public TradeOrderingException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of OrderingSystemException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public TradeOrderingException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class TradeOrderingException

} // namespace Empiria.Trade.Ordering