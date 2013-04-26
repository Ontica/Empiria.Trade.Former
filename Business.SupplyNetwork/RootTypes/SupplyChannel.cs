///* Empiria® Business Framework 2013 **************************************************************************
//*                                                                                                            *
//*  Solution  : Empiria® Business Framework                      System   : Supply Network Management         *
//*  Namespace : Empiria.SupplyNetwork                            Assembly : Empiria.SupplyNetwork.dll         *
//*  Type      : SupplyChannel                                    Pattern  : General Object Type               *
//*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
//*                                                                                                            *
//*  Summary   : Describes a supply channel like Internet, telephone, catalogue or store.                      *
//*                                                                                                            *
//**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1994-2013. **/

namespace Empiria.SupplyNetwork {

  /// <summary>Describes a supply channel like Internet, telephone, catalogue or store.</summary>
  public class SupplyChannel : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.SupplyChannel";

    #endregion Fields

    #region Constructors and parsers

    public SupplyChannel()
      : base(thisTypeName) {

    }

    protected SupplyChannel(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public SupplyChannel Empty {
      get { return BaseObject.ParseEmpty<SupplyChannel>(thisTypeName); }
    }

    static public SupplyChannel Parse(int id) {
      return BaseObject.Parse<SupplyChannel>(thisTypeName, id);
    }

    static public ObjectList<SupplyChannel> GetList() {
      ObjectList<SupplyChannel> list = GeneralObject.ParseList<SupplyChannel>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

  } // class SupplyChannel

} // namespace Empiria.SupplyNetwork