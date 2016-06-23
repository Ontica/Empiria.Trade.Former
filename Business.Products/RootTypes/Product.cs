/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                     System   : Product Data Management             *
*  Namespace : Empiria.Products                               Assembly : Empiria.Products.dll                *
*  Type      : Product                                        Pattern  : Partitioned type                    *
*  Version   : 2.1                                            License  : Please read license.txt file        *
*                                                                                                            *
*  Summary   : Abstract partitioned type that represents a physical good or service.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Geography;
using Empiria.Ontology;
using Empiria.Products.Data;

namespace Empiria.Products {

  public enum PackagingType {
    Empty = 'U',            // Undefined
    Bulk = 'K',             // Bulk (like screws or nails)
    Item = 'I',             // Primary (Item)
    Box = 'B',              // Secondary (Box)
    Pallet = 'P',           // Terciary (Pallet)
    NotApply = 'N',         // Not applied (services, documents, electronic, ...)
  }

  public enum IdentificationLevelType {
    Empty = 'U',            // Undefined
    SKU = 'K',              // Using SKU or UPC - barcode
    BatchNumber = 'B',      // (Per lot number)
    SerialNumber = 'S',     // Serial number. (Per each or per item).
    NotHandled = 'N',       // N = Not handled product (e.g., services, e-deliveried)
  }

  /// <summary>Abstract partitioned type that represents a physical good or service.</summary>
  [PartitionedType(typeof(ProductType))]
  public class Product : BaseObject {

    #region Constructors and parsers

    protected Product(ProductType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public Product Parse(int id) {
      return BaseObject.ParseId<Product>(id);
    }

    static public Product Empty {
      get { return BaseObject.ParseEmpty<Product>(); }
    }

    static public FixedList<Product> GetList(string keywords) {
      return ProductsData.GetActiveProducts(keywords, String.Empty);
    }

    #endregion Constructors and parsers

    #region Properties

    public ProductType ProductType {
      get {
        return (ProductType) base.GetEmpiriaType();
      }
    }

    [DataField("ProductTermId")]
    public ProductTerm ProductTerm {
      get;
      private set;
    }

    [DataField("ProductManagerId")]
    public Contact Manager {
      get;
      set;
    }

    [DataField("IsService")]
    public bool IsService {
      get;
      private set;
    }

    [DataField("IsCompound")]
    public bool IsCompound {
      get;
      private set;
    }

    [DataField("IsCustomizable")]
    public bool IsCustomizable {
      get;
      private set;
    }

    [DataField("BaseProductId")]
    private LazyInstance<Product> _baseProduct = LazyInstance<Product>.Empty;
    public Product BaseProduct {
      get {
        return _baseProduct.Value;
      }
      private set {
        _baseProduct = LazyInstance<Product>.Parse(value);
      }
    }

    public bool IsEquivalent {
      get {
        return !_baseProduct.IsEmptyInstance;
      }
    }

    [DataField("ManufacturerId")]
    public Manufacturer Manufacturer {
      get;
      set;
    }

    [DataField("BrandId")]
    public Brand Brand {
      get;
      private set;
    }

    [DataField("Model")]
    public string Model {
      get;
      set;
    }

    [DataField("PartNumber")]
    public string PartNumber {
      get;
      private set;
    }

    [DataField("ProductName")]
    public string Name {
      get;
      private set;
    }

    [DataField("SearchTags")]
    public string SearchTags {
      get;
      set;
    }

    [DataField("Description")]
    public string Description {
      get;
      set;
    }

    [DataField("Notes")]
    public string Notes {
      get;
      set;
    }

    [DataField("ProductExtData")]
    public string ExtendedData {
      get;
      protected set;
    }

    [DataField("ProductKeywords")]
    public string Keywords {
      get;
      protected set;
    }

    [DataField("PresentationId")]
    public PresentationUnit PresentationUnit {
      get;
      set;
    }

    [DataField("ContentsQty")]
    public decimal ContentsQty {
      get;
      set;
    }

    [DataField("ContentsUnitId")]
    public Unit ContentsUnit {
      get;
      set;
    }

    [DataField("PackagingType", Default = PackagingType.Empty)]
    public PackagingType PackagingType {
      get;
      set;
    } = PackagingType.Empty;

    [DataField("IdentificationLevel", Default = IdentificationLevelType.Empty)]
    public IdentificationLevelType IdentificationLevel {
      get;
      set;
    }

    [DataField("BarCodeID")]
    public string BarCodeID {
      get;
      set;
    }

    [DataField("StartDate")]
    public DateTime StartDate {
      get;
      private set;
    }

    [DataField("LastUpdated")]
    public DateTime LastUpdated {
      get;
      private set;
    }

    [DataField("ProductStatus", Default = GeneralObjectStatus.Active)]
    public GeneralObjectStatus Status {
      get;
      private set;
    }

    public string LegacyKey {
      get {
        return this.PartNumber + "@" + this.Brand.LegacyId;
      }
    }

    private Lazy<List<Product>> equivalentProducts = null;
    public FixedList<Product> Equivalents {
      get {
        return equivalentProducts.Value.ToFixedList();
      }
    }

    #endregion Properties

    #region Public methods

    public void AddEquivalent(Product equivalentProduct) {
      Assertion.Assert(!this.IsEmptyInstance, "Base product can't be the empty instance.");
      Assertion.AssertObject(equivalentProduct, "equivalentProduct");
      Assertion.Assert(!equivalentProduct.Equals(this),
                       "Target product should be distinct than the base product.");
      Assertion.Assert(!equivalentProducts.Value.Exists((x) => x.Id == equivalentProduct.Id),
                       "Target product was already registered as an equivalent product for the base product.");
      ProductsData.AddEquivalent(this, equivalentProduct);

      equivalentProducts.Value.Add(equivalentProduct);
    }

    protected override void OnInitialize() {
      this.equivalentProducts = new Lazy<List<Product>>(() => ProductsData.GetEquivalentProducts(this));
    }

    protected override void OnSave() {
      this.Keywords = "@" + this.PartNumber + "@ " +
                      ((this.BarCodeID.Length != 0) ? "@" + this.BarCodeID + "@ " : String.Empty) +
                      this.SearchTags +
                      EmpiriaString.BuildKeywords(this.Name, this.Brand.Name, this.Manufacturer.Name, this.Description);
      ProductsData.WriteProduct(this);
    }

    public void RemoveEquivalent(Product equivalentProduct) {
      Assertion.Assert(equivalentProducts.Value.Exists((x) => x.Id == equivalentProduct.Id),
                       "Target product is not an equivalent product for the base product.");

      ProductsData.RemoveEquivalent(this, equivalentProduct);

      equivalentProducts.Value.Remove(equivalentProduct);
    }

    #endregion Public methods

  } // class Product

} // namespace Empiria.Products
