/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : PresentationUnit                                 Pattern  : Storage Item                      *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Type that represents a product presentation or packaging unit.                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
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

    static public new PresentationUnit Parse(string presentationUnitNamedKey) {
      return BaseObject.ParseKey<PresentationUnit>(presentationUnitNamedKey);
    }

    static public new PresentationUnit Empty {
      get { return BaseObject.ParseEmpty<PresentationUnit>(); }
    }

    static public FixedList<PresentationUnit> GetList() {
      return GeneralObject.ParseList<PresentationUnit>();
    }

    #endregion Constructors and parsers

    #region Properties

    public FixedList<Unit> GetContentsUnits() {
      FixedList<Unit> list = base.GetLinks<Unit>("PresentationUnit_ContentsUnits");

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Properties

  } // class PresentationUnit

} // namespace Empiria.Products
