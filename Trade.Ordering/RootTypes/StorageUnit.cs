/* Empiria Trade 2014 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : StorageUnit                                      Pattern  : Empiria Object Type               *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Describes a warehouse storage unit.                                                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Products;

namespace Empiria.Trade.Ordering {

  public enum StorageUnitClass {
    Undefined = 'U',
    Area = 'A',
    Row = 'R',
    Aisle = 'E',
    Bin = 'B',
    BinSide = 'S',
    BinLevel = 'L',
    BinColumn = 'N',
    Cube = 'C',
    ProductReference = 'P',
  }

  /// <summary>Describes a warehouse storage unit.</summary>
  public class StorageUnit : BaseObject {

    #region Fields

    private Contact supplyPoint = Person.Empty;
    private StorageUnitKind kind = StorageUnitKind.Empty;
    private StorageUnitClass storageUnitClass = StorageUnitClass.Undefined;
    private string code = String.Empty;
    private string fullCode = String.Empty;
    private string description = String.Empty;
    private int areaId = -1;
    private StorageUnit area = null;
    private int rowId = -1;
    private StorageUnit row = null;
    private int aisleId = -1;
    private StorageUnit aisle = null;
    private int binId = -1;
    private StorageUnit bin = null;
    private int binSideId = -1;
    private StorageUnit binSide = null;
    private int binLevelId = -1;
    private StorageUnit binLevel = null;
    private int binColumnId = -1;
    private StorageUnit binColumn = null;
    private int cubeId = -1;
    private StorageUnit cube = null;
    private Product product = Product.Empty;
    private decimal monthlyCost = decimal.Zero;
    private decimal length = decimal.Zero;
    private decimal height = decimal.Zero;
    private decimal width = decimal.Zero;
    private Unit sizeUnit = Unit.Empty;
    private decimal weight = decimal.Zero;
    private Unit weightUnit = Unit.Empty;
    private string keywords = String.Empty;
    private int parentId = -1;
    private StorageUnit parent = null;
    private Contact postedBy = Person.Empty;
    private DateTime postingTime = DateTime.Now;
    private GeneralObjectStatus status = GeneralObjectStatus.Active;

    #endregion Fields

    #region Constuctors and parsers

    private StorageUnit() {
      // Required by Empiria Framework.
    }

    static public StorageUnit Parse(int id) {
      return BaseObject.ParseId<StorageUnit>(id);
    }

    static internal StorageUnit Parse(DataRow dataRow) {
      return BaseObject.ParseDataRow<StorageUnit>(dataRow);
    }

    static public StorageUnit Empty {
      get { return BaseObject.ParseEmpty<StorageUnit>(); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public Contact SupplyPoint {
      get { return supplyPoint; }
      set { supplyPoint = value; }
    }

    public StorageUnitKind Kind {
      get { return kind; }
      set { kind = value; }
    }

    public StorageUnitClass StorageUnitClass {
      get { return storageUnitClass; }
      set { storageUnitClass = value; }
    }

    public string Code {
      get { return code; }
      set { code = value; }
    }

    public string FullCode {
      get { return fullCode; }
      set { fullCode = value; }
    }

    public string Description {
      get { return description; }
      set { description = value; }
    }

    public StorageUnit Area {
      get {
        if (area == null) {
          area = StorageUnit.Parse(areaId);
        }
        return area;
      }
      set {
        area = value;
        areaId = area.Id;
      }
    }

    public StorageUnit Row {
      get {
        if (row == null) {
          row = StorageUnit.Parse(rowId);
        }
        return row;
      }
      set {
        row = value;
        rowId = row.Id;
      }
    }

    public StorageUnit Aisle {
      get {
        if (aisle == null) {
          aisle = StorageUnit.Parse(aisleId);
        }
        return aisle;
      }
      set {
        aisle = value;
        aisleId = aisle.Id;
      }
    }

    public StorageUnit Bin {
      get {
        if (bin == null) {
          bin = StorageUnit.Parse(binId);
        }
        return bin;
      }
      set {
        bin = value;
        binId = bin.Id;
      }
    }

    public StorageUnit BinSide {
      get {
        if (binSide == null) {
          binSide = StorageUnit.Parse(binSideId);
        }
        return binSide;
      }
      set {
        binSide = value;
        binSideId = binSide.Id;
      }
    }

    public StorageUnit BinLevel {
      get {
        if (binLevel == null) {
          binLevel = StorageUnit.Parse(binLevelId);
        }
        return binLevel;
      }
      set {
        binLevel = value;
        binLevelId = binLevel.Id;
      }
    }

    public StorageUnit BinColumn {
      get {
        if (binColumn == null) {
          binColumn = StorageUnit.Parse(binColumnId);
        }
        return binColumn;
      }
      set {
        binColumn = value;
        binColumnId = binColumn.Id;
      }
    }

    public StorageUnit Cube {
      get {
        if (cube == null) {
          cube = StorageUnit.Parse(cubeId);
        }
        return cube;
      }
      set {
        cube = value;
        cubeId = cube.Id;
      }
    }

    public Product Product {
      get { return product; }
      set { product = value; }
    }

    public decimal MonthlyCost {
      get { return monthlyCost; }
      set { monthlyCost = value; }
    }

    public decimal Length {
      get { return length; }
      set { length = value; }
    }

    public decimal Height {
      get { return height; }
      set { height = value; }
    }

    public decimal Width {
      get { return width; }
      set { width = value; }
    }

    public Unit SizeUnit {
      get { return sizeUnit; }
      set { sizeUnit = value; }
    }

    public decimal Weight {
      get { return weight; }
      set { weight = value; }
    }

    public Unit WeightUnit {
      get { return weightUnit; }
      set { weightUnit = value; }
    }

    public StorageUnit Parent {
      get {
        if (parent == null) {
          parent = StorageUnit.Parse(parentId);
        }
        return parent;
      }
      set {
        parent = value;
        parentId = parent.Id;
      }
    }

    public string Keywords {
      get { return keywords; }
      set { keywords = value; }
    }

    public DateTime PostingTime {
      get { return postingTime; }
      set { postingTime = value; }
    }

    public Contact PostedBy {
      get { return postedBy; }
      set { postedBy = value; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnLoadObjectData(DataRow row) {
      supplyPoint = Contact.Parse((int) row["SupplyPointId"]);
      kind = StorageUnitKind.Parse((int) row["StorageUnitKindId"]);
      storageUnitClass = (StorageUnitClass) Convert.ToChar(row["StorageUnitClass"]);
      code = (string) row["StorageUnitCode"];
      fullCode = (string) row["StorageUnitDescription"];
      description = (string) row["StorageUnitDescription"];
      areaId = (int) row["AreaId"];
      rowId = (int) row["RowId"];
      aisleId = (int) row["AisleId"];
      binId = (int) row["BinId"];
      binSideId = (int) row["BinSideId"];
      binLevelId = (int) row["BinLevelId"];
      binColumnId = (int) row["BinColumnId"];
      cubeId = (int) row["CubeId"];
      product = Product.Parse((int) row["ProductId"]);
      monthlyCost = (decimal) row["MonthlyCost"];
      length = (decimal) row["StorageLength"];
      height = (decimal) row["StorageHeight"];
      width = (decimal) row["StorageWidth"];
      sizeUnit = Unit.Parse((int) row["StorageSizeUnitId"]);
      weight = (decimal) row["StorageWeight"];
      weightUnit = Unit.Parse((int) row["StorageWeightUnitId"]);
      //keywords = (string) row[""];
      parentId = (int) row["ParentStorageUnitId"];
      postedBy = Contact.Parse((int) row["PostedById"]);
      postingTime = (DateTime) row["PostingTime"];
      status = (GeneralObjectStatus) Convert.ToChar(row["StorageUnitStatus"]);
    }

    #endregion Public methods

  } // class StorageUnit

} // namespace Empiria.Trade.Ordering
