/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive                             Assembly : Empiria.Automotive.dll              *
*  Type      : VehicleModel                                   Pattern  : Empiria Object Type                 *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Represents a vehicle model, with trim level and engine.                                       *
*                                                                                                            *
********************************* Copyright (c) 2008-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.StateEnums;

namespace Empiria.Automotive {

  /// <summary>Represents a vehicle model, with trim level and engine.</summary>
  public class VehicleModel : BaseObject {

    #region Constructors and parsers

    private VehicleModel() {
      // Required by Empiria Framework.
    }


    static public VehicleModel Parse(int id) {
      return BaseObject.ParseId<VehicleModel>(id);
    }


    static public VehicleModel Empty {
      get {
        return BaseObject.ParseEmpty<VehicleModel>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("MakeId")]
    public Make Make {
      get;
      private set;
    } = Make.Empty;


    [DataField("FromYear")]
    public int FromYear {
      get;
      private set;
    } = -1;


    [DataField("ToYear")]
    public int ToYear {
      get;
      private set;
    } = -1;


    [DataField("Model")]
    public string Model {
      get;
      private set;
    } = String.Empty;


    [DataField("TrimLevel")]
    public string TrimLevel {
      get;
      private set;
    } = String.Empty;


    [DataField("Engine")]
    public string Engine {
      get;
      private set;
    } = String.Empty;


    [DataField("AsText")]
    public string AsText {
      get;
      private set;
    } = String.Empty;


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    } = String.Empty;


    [DataField("Description")]
    public string Description {
      get;
      private set;
    } = String.Empty;


    [DataField("ExtensionData")]
    internal Json.JsonObject ExtensionData {
      get;
      private set;
    } = Json.JsonObject.Empty;


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.AsText);
      }
    }


    [DataField("Status", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      private set;
    }


    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class VehicleModel

} // namespace Empiria.Automotive
