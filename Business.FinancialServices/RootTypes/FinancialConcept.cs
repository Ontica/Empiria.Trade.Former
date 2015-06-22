﻿/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialConcept                                 Pattern  : Business Services Class           *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a financial account concept.                                                       *
*                                                                                                            *
********************************* Copyright (c) 2003-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.FinancialServices {

  /// <summary>Represents a financial account concept.</summary>
  public class FinancialConcept : GeneralObject {

    #region Constructors and parsers

    private FinancialConcept() {
      // Required by Empiria Framework.
    }

    static public FinancialConcept Empty {
      get { return BaseObject.ParseEmpty<FinancialConcept>(); }
    }

    static public FinancialConcept Parse(int id) {
      return BaseObject.ParseId<FinancialConcept>(id);
    }

    static public FinancialConcept Parse(string itemNamedKey) {
      return BaseObject.ParseKey<FinancialConcept>(itemNamedKey);
    }

    static public FixedList<FinancialConcept> GetList() {
      return GeneralObject.ParseList<FinancialConcept>();
    }

    #endregion Constructors and parsers

    #region Properties

    public bool AppliesToCredit {
      get { return (base.NamedKey.StartsWith("FSB") || base.NamedKey.StartsWith("FSC")); }
    }

    public bool AppliesToDebit {
      get { return (base.NamedKey.StartsWith("FSD") || base.NamedKey.StartsWith("FSP")); }
    }

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class FinancialConcept

} // namespace Empiria.FinancialServices
