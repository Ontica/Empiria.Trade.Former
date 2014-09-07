/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : FinancialInstitution                             Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a financial institution like a bank or credit agency.                              *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
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
      FixedList<FinancialInstitution> list = GeneralObject.ParseList<FinancialInstitution>();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class FinancialInstitution

} // namespace Empiria.Treasury
