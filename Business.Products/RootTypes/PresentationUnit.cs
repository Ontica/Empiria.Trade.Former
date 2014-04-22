﻿/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : PresentationUnit                                 Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Type that represents a product presentation or packaging unit.                                *
*                                                                                                            *
********************************* Copyright (c) 2009-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

using Empiria.DataTypes;

namespace Empiria.Products {

  public class PresentationUnit : Unit {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.Unit.ProductPresentationUnit";

    #endregion Fields

    #region Constructors and parsers

    public PresentationUnit()
      : base(thisTypeName) {

    }

    protected PresentationUnit(string typeName)
      : base(typeName) {

    }

    static public new PresentationUnit Parse(int id) {
      return BaseObject.Parse<PresentationUnit>(thisTypeName, id);
    }

    static public new PresentationUnit Parse(string presentationUnitNamedKey) {
      return BaseObject.Parse<PresentationUnit>(thisTypeName, presentationUnitNamedKey);
    }

    static public new PresentationUnit Empty {
      get { return BaseObject.ParseEmpty<PresentationUnit>(thisTypeName); }
    }

    static public ObjectList<PresentationUnit> GetList() {
      ObjectList<PresentationUnit> list = GeneralObject.ParseList<PresentationUnit>(thisTypeName);

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Constructors and parsers

    #region Properties

    public ObjectList<Unit> GetContentsUnits() {
      ObjectList<Unit> list = base.GetLinks<Unit>("PresentationUnit_ContentsUnits");

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      return list;
    }

    #endregion Properties

  } // class PresentationUnit

} // namespace Empiria.Products