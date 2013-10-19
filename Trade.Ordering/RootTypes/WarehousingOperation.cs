/* Empiria® Trade 2013 ***************************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Trade                                   System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : WarehousingOperation                             Pattern  : General Object Type               *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Describes a warehousing operation.                                                            *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;

namespace Empiria.Trade.Ordering {

  /// <summary>Describes a warehousing operation.</summary>
  public class WarehousingOperation : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.WarehousingOperation";

    #endregion Fields

    #region Constructors and parsers

    public WarehousingOperation()
      : base(thisTypeName) {

    }

    protected WarehousingOperation(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public WarehousingOperation Empty {
      get { return BaseObject.ParseEmpty<WarehousingOperation>(thisTypeName); }
    }

    static public WarehousingOperation Parse(int id) {
      return BaseObject.Parse<WarehousingOperation>(thisTypeName, id);
    }

    static public ObjectList<WarehousingOperation> GetList() {
      ObjectList<WarehousingOperation> list = GeneralObject.ParseList<WarehousingOperation>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    public ObjectList<WarehousingOperation> GetDocumentTypes() {
      ObjectList<WarehousingOperation> list = this.GetLinks<WarehousingOperation>("TransactionType_DocumentType");

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class WarehousingOperation

} // namespace Empiria.Trade.Ordering