/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                  System   : Automotive Industry Services        *
*  Namespace : Empiria.Automotive                             Assembly : Empiria.Automotive.dll              *
*  Type      : Make                                           Pattern  : Storage Item                        *
*  Version   : 2.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Describes a vehicle make.                                                                     *
*                                                                                                            *
********************************* Copyright (c) 2008-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Automotive {

  /// <summary>Describes a vehicle make.</summary>
  public class Make : GeneralObject {

    #region Constructors and parsers

    private Make() {
      // Required by Empiria Framework.
    }

    static public Make Parse(int id) {
      return BaseObject.ParseId<Make>(id);
    }

    static public Make Parse(string key) {
      return BaseObject.ParseKey<Make>(key);
    }

    static public Make Empty {
      get { return BaseObject.ParseEmpty<Make>(); }
    }

    static public FixedList<Make> GetList() {
      var list = GeneralObject.ParseList<Make>();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public string Key {
      get {
        return base.NamedKey;
      }
      private set {
        base.NamedKey = value;
      }
    }

    #endregion Properties

  } // class Make

} // namespace Empiria.Automotive
