///* Empiria® Business Framework 2013 **************************************************************************
//*                                                                                                            *
//*  Solution  : Empiria® Business Framework                      System   : Supply Network Management         *
//*  Namespace : Empiria.SupplyNetwork                            Assembly : Empiria.SupplyNetwork.dll         *
//*  Type      : DeliveryMode                                     Pattern  : General Object Type               *
//*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
//*                                                                                                            *
//*  Summary   : Represents a delivery type or condition like not delivery, store, pick, air, land.            *
//*                                                                                                            *
//**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.SupplyNetwork {

  /// <summary>Represents a delivery type or condition like not delivery, store, pick, air, land.</summary>
  public class DeliveryMode : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.DeliveryMode";

    #endregion Fields

    #region Constructors and parsers

    public DeliveryMode()
      : base(thisTypeName) {

    }

    protected DeliveryMode(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public DeliveryMode Empty {
      get { return BaseObject.ParseEmpty<DeliveryMode>(thisTypeName); }
    }

    static public DeliveryMode Parse(int id) {
      return BaseObject.Parse<DeliveryMode>(thisTypeName, id);
    }

    static public ObjectList<DeliveryMode> GetList() {
      ObjectList<DeliveryMode> list = GeneralObject.ParseList<DeliveryMode>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public bool IsNoDeliveryMode {
      get { return (base.NamedKey == "N"); }
    }

    public bool IsCargoDeliveryMode {
      get { return (base.NamedKey == "E"); }
    }

    public new string NamedKey {
      get { return base.NamedKey; }
    }

    #endregion Properties

  } // class DeliveryMode

} // namespace Empiria.SupplyNetwork