/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : FinancialInstitution                             Pattern  : Ontology Object Type              *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a financial institution like a bank or credit agency.                              *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

namespace Empiria.Treasury {

  /// <summary>Represents a financial institution like a bank or credit agency.</summary>
  public class FinancialInstitution : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.FinancialInstitution";

    #endregion Fields

    #region Constructors and parsers

    public FinancialInstitution()
      : base(thisTypeName) {

    }

    protected FinancialInstitution(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public FinancialInstitution Default {
      get { return FinancialInstitution.Parse(50); }
    }

    static public FinancialInstitution Empty {
      get { return BaseObject.ParseEmpty<FinancialInstitution>(thisTypeName); }
    }

    static public FinancialInstitution Parse(int id) {
      return BaseObject.Parse<FinancialInstitution>(thisTypeName, id);
    }

    static public FinancialInstitution Parse(string itemNamedKey) {
      return BaseObject.Parse<FinancialInstitution>(thisTypeName, itemNamedKey);
    }

    static public ObjectList<FinancialInstitution> GetList() {
      ObjectList<FinancialInstitution> list = GeneralObject.ParseList<FinancialInstitution>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class FinancialInstitution

} // namespace Empiria.Treasury
