/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Product                                          Pattern  : Storage Item                      *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Abstract type that represents a physical good or service.                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.DataTypes;
using Empiria.Geography;

using Empiria.Products.Data;

namespace Empiria.Products {

  public enum PackagingType {
    Undefined = 'U',        // Undefined
    Bulk = 'K',             // Bulk (like screws or nails)
    Item = 'I',             // Primary (Item)
    Box = 'B',              // Secondary (Box)
    Pallet = 'P',           // Terciary (Pallet)
    NotApply = 'N',         // Not applied (services, documents, electronic, ...)
  }

  public enum IdentificationLevelType {
    Undefined = 'U',        // Undefined
    SKU = 'K',              // Using SKU or UPC - barcode
    BatchNumber = 'B',      // (Per lot number)
    SerialNumber = 'S',     // Serial number. (Per each or per item).
    NotHandled = 'N',       // N = Not handled product (e.g., services, e-deliveried)
  }

  /// <summary>Abstract type that represents a physical good or service.</summary>
  public class Product : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.Product";

    private ProductTerm productTerm = ProductTerm.Empty;
    private Contact manager = Person.Empty;
    private bool isService = false;
    private bool isCompound = false;
    private bool isCustomizable = false;
    private bool needsReview = false;
    private Manufacturer manufacturer = Manufacturer.Empty;
    private Brand brand = Brand.Empty;
    private GeographicRegionItem originCountry = GeographicRegionItem.Empty;
    private string model = String.Empty;
    private string partNumber = String.Empty;
    private string name = String.Empty;
    private string imageFile = String.Empty;
    private string smallImageFile = String.Empty;
    private string searchTags = String.Empty;
    private string specification = String.Empty;
    private string notes = String.Empty;
    private string keywords = String.Empty;
    private PresentationUnit presentationUnit = PresentationUnit.Empty;
    private decimal contentsQty = decimal.Zero;
    private Unit contentsUnit = Unit.Empty;
    private PackagingType packagingType = PackagingType.Item;
    private IdentificationLevelType identificationLevel = IdentificationLevelType.SKU;
    private string barCodeID = String.Empty;
    private string radioFrequenceID = String.Empty;
    private decimal lengthSize = decimal.Zero;
    private decimal widthSize = decimal.Zero;
    private decimal heightSize = decimal.Zero;
    private Unit sizeUnit = Unit.Empty;
    private decimal weight = decimal.Zero;
    private Unit weightUnit = Unit.Empty;
    private Contact reviewedBy = Person.Empty;
    private Contact postedBy = Person.Empty;
    private int replacedById = 0;
    private GeneralObjectStatus status = GeneralObjectStatus.Active;
    private DateTime startDate = DateTime.Today;
    private DateTime endDate = ExecutionServer.DateMaxValue;
    private string legacyKey = String.Empty;

    #endregion Fields

    #region Constructors and parsers

    public Product()
      : base(thisTypeName) {
      // Abstract class. Object creation of this type not allowed.
    }

    protected Product(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public Product Parse(int id) {
      return BaseObject.Parse<Product>(thisTypeName, id);
    }

    static public Product ParseFromBelow(int id) {
      return BaseObject.ParseFromBelow<Product>(thisTypeName, id);
    }

    static internal Product Parse(DataRow dataRow) {
      return BaseObject.Parse<Product>(thisTypeName, dataRow);
    }

    static public Product Empty {
      get { return BaseObject.ParseEmpty<Product>(thisTypeName); }
    }

    static public ObjectList<Product> GetList(string keywords) {
      DataTable table = ProductsData.GetProducts(keywords, String.Empty);

      return new ObjectList<Product>((x) => Product.Parse(x), table);
    }

    #endregion Constructors and parsers

    #region Properties

    public ProductTerm ProductTerm {
      get { return productTerm; }
      set { productTerm = value; }
    }

    public Contact Manager {
      get { return manager; }
      set { manager = value; }
    }

    public bool IsService {
      get { return isService; }
      set { isService = value; }
    }

    public bool IsCompound {
      get { return isCompound; }
      set { isCompound = value; }
    }

    public bool IsCustomizable {
      get { return isCustomizable; }
      set { isCustomizable = value; }
    }

    public bool NeedsReview {
      get { return needsReview; }
      set { needsReview = value; }
    }

    public Manufacturer Manufacturer {
      get { return manufacturer; }
      set { manufacturer = value; }
    }

    public Brand Brand {
      get { return brand; }
      set {
        if (base.IsNew) {
          this.brand = value;
          this.legacyKey = partNumber + "@" + brand.LegacyId.ToString();
        }
      }
    }

    public GeographicRegionItem OriginCountry {
      get { return originCountry; }
      set { originCountry = value; }
    }

    public string Model {
      get { return model; }
      set { model = value; }
    }

    public string PartNumber {
      get { return partNumber; }
      set {
        if (base.IsNew) {
          this.partNumber = value;
          this.legacyKey = partNumber + "@" + brand.LegacyId.ToString();
        }
      }
    }

    public string Name {
      get { return name; }
      set { name = EmpiriaString.TrimAll(value); }
    }

    public string ExtendedName {
      get { return "[" + PartNumber + "] " + Name + " / " + Brand.Name; }
    }

    public string ImageFile {
      get { return imageFile; }
      set { imageFile = value; }
    }

    public string SmallImageFile {
      get { return smallImageFile; }
      set { smallImageFile = value; }
    }

    public string SearchTags {
      get { return searchTags; }
      set { searchTags = EmpiriaString.TrimAll(value); }
    }

    public string Specification {
      get { return specification; }
      set { specification = EmpiriaString.TrimAll(value); }
    }

    public string Notes {
      get { return notes; }
      set { notes = EmpiriaString.TrimAll(value); }
    }

    public string Keywords {
      get { return keywords; }
      protected set { keywords = value; }
    }

    public PresentationUnit PresentationUnit {
      get { return presentationUnit; }
      set { presentationUnit = value; }
    }

    public decimal ContentsQty {
      get { return contentsQty; }
      set { contentsQty = value; }
    }

    public Unit ContentsUnit {
      get { return contentsUnit; }
      set { contentsUnit = value; }
    }

    public PackagingType PackagingType {
      get { return packagingType; }
      set { packagingType = value; }
    }

    public IdentificationLevelType IdentificationLevel {
      get { return identificationLevel; }
      set { identificationLevel = value; }
    }

    public string BarCodeID {
      get { return barCodeID; }
      set { barCodeID = EmpiriaString.TrimAll(value); }
    }

    public string RadioFrequenceID {
      get { return radioFrequenceID; }
      set { radioFrequenceID = EmpiriaString.TrimAll(value); }
    }

    public decimal LengthSize {
      get { return lengthSize; }
      set { lengthSize = value; }
    }

    public decimal WidthSize {
      get { return widthSize; }
      set { widthSize = value; }
    }

    public decimal HeightSize {
      get { return heightSize; }
      set { heightSize = value; }
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

    public Contact ReviewedBy {
      get { return reviewedBy; }
    }

    public Contact PostedBy {
      get { return postedBy; }
    }

    public int ReplacedById {
      get { return replacedById; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    public DateTime StartDate {
      get { return startDate; }
    }

    public DateTime EndDate {
      get { return endDate; }
    }

    public string LegacyKey {
      get { return legacyKey; }
    }

    #endregion Properties

    #region Public methods

    protected override void ImplementsLoadObjectData(DataRow row) {
      productTerm = ProductTerm.Parse((int) row["ProductTermId"]);
      manager = Contact.Parse((int) row["ProductManagerId"]);
      isService = (bool) row["IsService"];
      isCompound = (bool) row["IsCompound"];
      isCustomizable = (bool) row["IsCustomizable"];
      needsReview = (bool) row["IsDirty"];
      manufacturer = Manufacturer.Parse((int) row["ManufacturerId"]);
      brand = Brand.Parse((int) row["BrandId"]);
      originCountry = GeographicRegionItem.Parse((int) row["OriginCountryId"]);
      model = (string) row["Model"];
      partNumber = (string) row["PartNumber"];
      name = (string) row["ProductName"];
      imageFile = (string) row["ProductImageFile"];
      smallImageFile = (string) row["ProductSmallImageFile"];
      searchTags = (string) row["SearchTags"];
      specification = (string) row["Specification"];
      notes = (string) row["Notes"];
      keywords = (string) row["ProductKeywords"];
      presentationUnit = PresentationUnit.Parse((int) row["PresentationId"]);
      contentsQty = (decimal) row["ContentsQty"];
      contentsUnit = Unit.Parse((int) row["ContentsUnitId"]);
      packagingType = (PackagingType) Convert.ToChar(row["PackagingType"]);
      identificationLevel = (IdentificationLevelType) Convert.ToChar(row["IdentificationLevel"]);
      barCodeID = (string) row["BarCodeID"];
      radioFrequenceID = (string) row["RadioFrequenceID"];
      lengthSize = (decimal) row["LengthSize"];
      widthSize = (decimal) row["WidthSize"];
      heightSize = (decimal) row["HeightSize"];
      sizeUnit = Unit.Parse((int) row["SizeUnitId"]);
      weight = (decimal) row["Weight"];
      weightUnit = Unit.Parse((int) row["WeightUnitId"]);
      reviewedBy = Contact.Parse((int) row["ReviewedById"]);
      postedBy = Contact.Parse((int) row["PostedById"]);
      replacedById = (int) row["ReplacedById"];
      status = (GeneralObjectStatus) Convert.ToChar(row["ProductStatus"]);
      startDate = (DateTime) row["StartDate"];
      endDate = (DateTime) row["EndDate"];
      legacyKey = (string) row["LegacyKey"];
    }

    protected override void ImplementsSave() {
      keywords = "@" + this.PartNumber + "@ " + ((this.BarCodeID.Length != 0) ? "@" + this.BarCodeID + "@ " : String.Empty) +
                 EmpiriaString.BuildKeywords(this.Name, this.Brand.Name, this.Manufacturer.Name, this.Specification);
      ProductsData.WriteProduct(this);
    }

    protected void SetPartNumberAndBrand(string newPartNumber, Products.Brand newBrand) {
      this.partNumber = newPartNumber;
      this.brand = newBrand;
      this.legacyKey = partNumber + "@" + brand.LegacyId.ToString();
    }

    #endregion Public methods

  } // class Product

} // namespace Empiria.Products