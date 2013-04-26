/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : InstrumentType                                   Pattern  : Ontology Object Type              *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents an financial instrument type.                                                      *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

namespace Empiria.Treasury {

  /// <summary>Represents an financial instrument type.</summary>
  public class InstrumentType : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.FinancialInstrumentType";

    #endregion Fields

    #region Constructors and parsers

    public InstrumentType()
      : base(thisTypeName) {

    }

    protected InstrumentType(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public InstrumentType Empty {
      get { return BaseObject.ParseEmpty<InstrumentType>(thisTypeName); }
    }

    static public InstrumentType Unknown {
      get { return BaseObject.ParseUnknown<InstrumentType>(thisTypeName); }
    }

    static public InstrumentType Multiple {
      get { return BaseObject.Parse<InstrumentType>(thisTypeName, "multiple"); }
    }

    static public InstrumentType Parse(int id) {
      return BaseObject.Parse<InstrumentType>(thisTypeName, id);
    }

    static public InstrumentType Parse(string itemNamedKey) {
      return BaseObject.Parse<InstrumentType>(thisTypeName, itemNamedKey);
    }

    static public ObjectList<InstrumentType> GetList() {
      ObjectList<InstrumentType> list = GeneralObject.ParseList<InstrumentType>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    public string TaxFormName {
      get { return base.Description; }
    }

    #endregion Properties

  } // class InstrumentType

} // namespace Empiria.Treasury