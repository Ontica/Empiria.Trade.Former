/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : CRTransactionType                                Pattern  : Ontology Object Type              *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a cash register transaction or operation type.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.Treasury {

  /// <summary>Represents a cash register transaction or operation type.</summary>
  public class CRTransactionType : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.CashRegisterTransactionType";

    #endregion Fields

    #region Constructors and parsers

    public CRTransactionType()
      : base(thisTypeName) {

    }

    protected CRTransactionType(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public CRTransactionType Empty {
      get { return BaseObject.ParseEmpty<CRTransactionType>(thisTypeName); }
    }

    static public CRTransactionType Unknown {
      get { return BaseObject.ParseUnknown<CRTransactionType>(thisTypeName); }
    }

    static public CRTransactionType Parse(int id) {
      return BaseObject.Parse<CRTransactionType>(thisTypeName, id);
    }

    static public CRTransactionType Parse(string itemNamedKey) {
      return BaseObject.Parse<CRTransactionType>(thisTypeName, itemNamedKey);
    }

    static public ObjectList<CRTransactionType> GetList() {
      ObjectList<CRTransactionType> list = GeneralObject.ParseList<CRTransactionType>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class CRTransactionType

} // namespace Empiria.Treasury