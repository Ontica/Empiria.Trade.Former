/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Financial Services Management     *
*  Namespace : Empiria.FinancialServices                        Assembly : Empiria.FinancialServices.dll     *
*  Type      : FinancialConcept                                 Pattern  : Business Services Class           *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a financial product like a credit, debit or check account.                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.FinancialServices {

  /// <summary>Represents a financial product like a credit, debit or check account.</summary>
  public class FinancialProduct : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.FinancialProduct";

    #endregion Fields

    #region Constructors and parsers

    public FinancialProduct()
      : base(thisTypeName) {

    }

    protected FinancialProduct(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public FinancialProduct Default {
      get { return FinancialProduct.Parse(95); }
    }

    static public FinancialProduct Parse(int id) {
      return BaseObject.Parse<FinancialProduct>(thisTypeName, id);
    }

    static public FinancialProduct Parse(string itemNamedKey) {
      return BaseObject.Parse<FinancialProduct>(thisTypeName, itemNamedKey);
    }

    static public ObjectList<FinancialProduct> GetList() {
      ObjectList<FinancialProduct> list = GeneralObject.ParseList<FinancialProduct>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
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