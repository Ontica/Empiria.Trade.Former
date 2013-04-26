/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialServicesException                       Pattern  : Empiria Exception Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in Empiria® Financial Services.            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/
using System;
using System.Reflection;

namespace Empiria.FinancialServices {

  /// <summary>The exception that is thrown when a problem occurs in Empiria® Financial Services.</summary>
  [Serializable]
  public sealed class FinancialServicesException : EmpiriaException {

    public enum Msg {
      ConverterIsRunning,
      CreditBalanceRebuildFails,
      PaymentDistributionError,
    }

    static private string resourceBaseName = "Empiria.FinancialServices.RootTypes.FinancialServicesExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of FinancialServicesException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public FinancialServicesException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {
      //base.Publish();
    }

    /// <summary>Initializes a new instance of FinancialServicesException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public FinancialServicesException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {
      //base.Publish();
    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class FinancialServicesException

} // namespace Empiria.Industries.Gas
