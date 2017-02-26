/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Financial Services                *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialConcept                                 Pattern  : Business Services Class           *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a financial product like a credit, debit or check account.                         *
*                                                                                                            *
********************************* Copyright (c) 2003-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.FinancialServices {

  /// <summary>Represents a financial product like a credit, debit or check account.</summary>
  public class FinancialProduct : GeneralObject {

    #region Constructors and parsers

    private FinancialProduct() {
      // Required by Empiria Framework.
    }

    static public FinancialProduct Default {
      get { return FinancialProduct.Parse(95); }
    }

    static public FinancialProduct Parse(int id) {
      return BaseObject.ParseId<FinancialProduct>(id);
    }

    static public FinancialProduct Parse(string itemNamedKey) {
      return BaseObject.ParseKey<FinancialProduct>(itemNamedKey);
    }

    static public FixedList<FinancialProduct> GetList() {
      return GeneralObject.ParseList<FinancialProduct>();
    }

    #endregion Constructors and parsers

    #region Properties

    public bool IsCredit {
      get { return base.NamedKey.StartsWith("FPCredit"); }
    }

    public bool IsDebitCheckingOrSavings {
      get { return (base.NamedKey.StartsWith("FPDebit") || base.NamedKey.StartsWith("FPChecking") || base.NamedKey.StartsWith("FPSavings")); }
    }

    #endregion Properties

  } // class FinancialProduct

} // namespace Empiria.FinancialServices
