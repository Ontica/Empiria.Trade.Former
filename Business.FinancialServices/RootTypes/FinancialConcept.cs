/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialConcept                                 Pattern  : Business Services Class           *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Represents a financial account concept.                                                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.FinancialServices {

  /// <summary>Represents a financial account concept.</summary>
  public class FinancialConcept : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.FinancialConcept";

    #endregion Fields

    #region Constructors and parsers

    public FinancialConcept()
      : base(thisTypeName) {

    }

    protected FinancialConcept(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public FinancialConcept Empty {
      get { return BaseObject.ParseEmpty<FinancialConcept>(thisTypeName); }
    }

    static public FinancialConcept Parse(int id) {
      return BaseObject.Parse<FinancialConcept>(thisTypeName, id);
    }

    static public FinancialConcept Parse(string itemNamedKey) {
      return BaseObject.Parse<FinancialConcept>(thisTypeName, itemNamedKey);
    }

    static public ObjectList<FinancialConcept> GetList() {
      ObjectList<FinancialConcept> list = GeneralObject.ParseList<FinancialConcept>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public bool AppliesToCredit {
      get { return (base.NamedKey.StartsWith("FSB") || base.NamedKey.StartsWith("FSC")); }
    }

    public bool AppliesToDebit {
      get { return (base.NamedKey.StartsWith("FSD") || base.NamedKey.StartsWith("FSP")); }
    }

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class FinancialConcept

} // namespace Empiria.FinancialServices