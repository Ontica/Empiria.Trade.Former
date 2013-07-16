/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Supply Network Management         *
*  Namespace : Empiria.SupplyNetwork                            Assembly : Empiria.SupplyNetwork.dll         *
*  Type      : SupplyNetworkException                           Pattern  : Empiria Exception Class           *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : The exception that is thrown when a problem occurs in the Supply Network Management System.   *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Reflection;

namespace Empiria.SupplyNetwork {

  /// <summary>The exception that is thrown when a problem occurs in the Supply Network Management System.</summary>
  [Serializable]
  public sealed class SupplyNetworkException : EmpiriaException {

    public enum Msg {
      InvalidCategoryRule,
      InvalidChildCategory,
      InvalidPeriodForDailyBills,
      UnrecognizedBillType,
    }

    static private string resourceBaseName = "Empiria.SupplyNetwork.RootTypes.SupplyNetworkExceptionMsg";

    #region Constructors and parsers

    /// <summary>Initializes a new instance of ProductManagementException class with a specified error 
    /// message.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public SupplyNetworkException(Msg message, params object[] args)
      : base(message.ToString(), GetMessage(message, args)) {

    }

    /// <summary>Initializes a new instance of ProductManagementException class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">Used to indicate the description of the exception.</param>
    /// <param name="innerException">This is the inner exception.</param>
    /// <param name="args">An optional array of items to format into the exception message.</param>
    public SupplyNetworkException(Msg message, Exception innerException, params object[] args)
      : base(message.ToString(), GetMessage(message, args), innerException) {

    }

    #endregion Constructors and parsers

    #region Private methods

    static private string GetMessage(Msg message, params object[] args) {
      return GetResourceMessage(message.ToString(), resourceBaseName, Assembly.GetExecutingAssembly(), args);
    }

    #endregion Private methods

  } // class SupplyNetworkException

} // namespace Empiria.SupplyNetwork