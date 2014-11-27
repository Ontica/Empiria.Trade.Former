/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : InstrumentType                                   Pattern  : Ontology Object Type              *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a financial instrument type.                                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Treasury {

  /// <summary>Represents a financial instrument type.</summary>
  public class InstrumentType : GeneralObject {

    #region Constructors and parsers

    private InstrumentType() {
      // Required by Empiria Framework.
    }

    static public InstrumentType Empty {
      get { return BaseObject.ParseEmpty<InstrumentType>(); }
    }

    static public InstrumentType Unknown {
      get { return BaseObject.ParseUnknown<InstrumentType>(); }
    }

    static public InstrumentType Multiple {
      get { return BaseObject.ParseKey<InstrumentType>("multiple"); }
    }

    static public InstrumentType Parse(int id) {
      return BaseObject.ParseId<InstrumentType>(id);
    }

    static public InstrumentType Parse(string itemNamedKey) {
      return BaseObject.ParseKey<InstrumentType>(itemNamedKey);
    }

    static public FixedList<InstrumentType> GetList() {
      return GeneralObject.ParseList<InstrumentType>();
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField(GeneralObject.ExtensionDataFieldName + ".TaxFormName")]
    public string TaxFormName {
      get;
      private set;
    }

    public string UniqueCode {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class InstrumentType

} // namespace Empiria.Treasury
