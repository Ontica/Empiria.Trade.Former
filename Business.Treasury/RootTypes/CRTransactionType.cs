/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : CRTransactionType                                Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a cash register transaction or operation type.                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Treasury {

  /// <summary>Represents a cash register transaction or operation type.</summary>
  public class CRTransactionType : GeneralObject {

    #region Constructors and parsers

    private CRTransactionType() {
      // Required by Empiria Framework.
    }

    static public CRTransactionType Empty {
      get { return BaseObject.ParseEmpty<CRTransactionType>(); }
    }

    static public CRTransactionType Unknown {
      get { return BaseObject.ParseUnknown<CRTransactionType>(); }
    }

    static public CRTransactionType Parse(int id) {
      return BaseObject.ParseId<CRTransactionType>(id);
    }

    static public CRTransactionType Parse(string itemNamedKey) {
      return BaseObject.ParseKey<CRTransactionType>(itemNamedKey);
    }

    static public FixedList<CRTransactionType> GetList() {
      return GeneralObject.ParseList<CRTransactionType>();
    }

    #endregion Constructors and parsers

    #region Properties

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class CRTransactionType

} // namespace Empiria.Treasury
