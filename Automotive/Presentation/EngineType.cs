/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive                             Assembly : Empiria.Automotive.dll              *
*  Type      : EngineType                                     Pattern  : Empiria Object Type                 *
*  Version   : 2.2                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Represents a vehicle engine type.                                                             *
*                                                                                                            *
********************************* Copyright (c) 2008-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.StateEnums;

namespace Empiria.Automotive {

  /// <summary>Represents a vehicle engine type.</summary>
  public class EngineType : BaseObject {

    #region Constructors and parsers

    private EngineType() {
      // Required by Empiria Framework.
    }


    static public EngineType Parse(int id) {
      return BaseObject.ParseId<EngineType>(id);
    }


    static public EngineType Empty {
      get {
        return BaseObject.ParseEmpty<EngineType>();
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("AsText")]
    public string AsText {
      get;
      private set;
    } = String.Empty;


    [DataField("Status", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      set;
    }


    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      throw new NotImplementedException();
    }

    #endregion Public methods

  } // class EngineType

} // namespace Empiria.Automotive
