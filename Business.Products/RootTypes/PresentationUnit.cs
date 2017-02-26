/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : PresentationUnit                                 Pattern  : Storage Item                      *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Type that represents a product presentation or packaging unit.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using Empiria.DataTypes;

namespace Empiria.Products {

  public class PresentationUnit : Unit {

    #region Constructors and parsers

    private PresentationUnit() {
      // Required by Empiria Framework.
    }

    static public new PresentationUnit Parse(int id) {
      return BaseObject.ParseId<PresentationUnit>(id);
    }

    static public new PresentationUnit Parse(string namedKey) {
      return BaseObject.ParseKey<PresentationUnit>(namedKey);
    }

    static public new PresentationUnit Empty {
      get { return BaseObject.ParseEmpty<PresentationUnit>(); }
    }

    static public FixedList<PresentationUnit> GetList() {
      return GeneralObject.ParseList<PresentationUnit>();
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField(ExtensionDataFieldName + ".IsBulkUnit", IsOptional = true, Default = false)]
    public bool IsBulkUnit {
      get;
      private set;
    }

    FixedList<Unit> _contentUnitsList = null;
    public FixedList<Unit> ContentsUnits() {
      if (_contentUnitsList == null) {
        var list = base.ExtendedDataField.GetList<Unit>("ContentUnits");
        list.Sort((x, y) => x.Name.CompareTo(y.Name));
        _contentUnitsList = list.ToFixedList();
      }
      return _contentUnitsList;
    }

    #endregion Properties

  } // class PresentationUnit

} // namespace Empiria.Products
