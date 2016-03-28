/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Treasury System                   *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : FinancialInstitution                             Pattern  : Ontology Object Type              *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a financial institution like a bank or credit agency.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Treasury {

  /// <summary>Represents a financial institution like a bank or credit agency.</summary>
  public class FinancialInstitution : GeneralObject {

    #region Constructors and parsers

    private FinancialInstitution(){
      // Required by Empiria Framework.
    }

    static public FinancialInstitution Default {
      get { return FinancialInstitution.Parse(50); }
    }

    static public FinancialInstitution Empty {
      get { return BaseObject.ParseEmpty<FinancialInstitution>(); }
    }

    static public FinancialInstitution Parse(int id) {
      return BaseObject.ParseId<FinancialInstitution>(id);
    }

    static public FinancialInstitution Parse(string itemNamedKey) {
      return BaseObject.ParseKey<FinancialInstitution>(itemNamedKey);
    }

    static public FixedList<FinancialInstitution> GetList() {
      return GeneralObject.ParseList<FinancialInstitution>();
    }

    #endregion Constructors and parsers

  } // class FinancialInstitution

} // namespace Empiria.Treasury
