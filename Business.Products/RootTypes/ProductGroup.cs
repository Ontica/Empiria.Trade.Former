/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductGroup                                     Pattern  : Storage Item                      *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a product grouping category.                                                       *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Products.Data;

namespace Empiria.Products {

  /// <summary>Represents a product grouping category.</summary>
  public class ProductGroup : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.ProductGroup";

    private string number = String.Empty;
    private string name = String.Empty;
    private string englishName = String.Empty;
    private string description = String.Empty;
    private string tags = String.Empty;
    private string keywords = String.Empty;
    private Contact manager = Person.Empty;
    private Contact modifiedBy = Person.Empty;
    private int parentId = -1;
    // use null initalization instead ProductGroup.Empty because is a fractal object and Empty instance 
    // parsing throws an infinite loop
    private ProductGroup parent = null;
    private GeneralObjectStatus status = GeneralObjectStatus.Pending;

    private string breadcrumb = null;

    #endregion Fields

    #region Constructors and parsers

    private ProductGroup()
      : base(thisTypeName) {
      // For create instances use Create static method instead
    }

    protected ProductGroup(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public ProductGroup Empty {
      get { return BaseObject.ParseEmpty<ProductGroup>(thisTypeName); }
    }

    static public ProductGroup Parse(int id) {
      return BaseObject.Parse<ProductGroup>(thisTypeName, id);
    }

    static internal ProductGroup Parse(DataRow dataRow) {
      return BaseObject.Parse<ProductGroup>(thisTypeName, dataRow);
    }

    static public ObjectList<ProductGroup> GetRoots() {
      return ProductsData.GetProductGroupChilds(ProductGroup.Empty);
    }

    #endregion Constructors and parsers

    #region Public properties

    public string Breadcrumb {
      get {
        if (breadcrumb == null) {
          ProductGroup current = this.Parent;
          while (true) {
            if (!String.IsNullOrEmpty(breadcrumb)) {
              breadcrumb = " » " + breadcrumb;
            }
            breadcrumb = current.Name + breadcrumb;
            if (!current.IsMainRoot) {
              current = current.Parent;
            } else {
              break;
            }
          }
        }
        return breadcrumb;
      }
    }

    public string Description {
      get { return description; }
      set { description = EmpiriaString.TrimAll(value); }
    }

    public string EnglishName {
      get { return englishName; }
      set { englishName = EmpiriaString.TrimAll(value); }
    }

    public bool IsMainRoot {
      get { return (this.Id == this.parentId); }
    }

    public string Keywords {
      get { return keywords; }
    }

    public Contact Manager {
      get { return manager; }
      set { manager = value; }
    }

    public Contact ModifiedBy {
      get { return modifiedBy; }
      set { modifiedBy = value; }
    }

    public string Name {
      get { return name; }
      set { name = EmpiriaString.TrimAll(value); }
    }

    public string Number {
      get { return number; }
      set { number = EmpiriaString.TrimAll(value); }
    }

    public ProductGroup Parent {
      get {
        if (parent == null) {
          parent = ProductGroup.Parse(parentId);
        }
        return parent;
      }
      internal set {
        parent = value;
        parentId = parent.Id;
      }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    public string StatusName {
      get {
        switch (status) {
          case GeneralObjectStatus.Pending:
            return "Pendiente";
          case GeneralObjectStatus.Suspended:
            return "Suspendida";
          case GeneralObjectStatus.Active:
            return "Activa";
          case GeneralObjectStatus.Deleted:
            return "Eliminada";
          default:
            return "No determinado";
        }
      } // get
    }

    public string Tags {
      get { return tags; }
      set { tags = EmpiriaString.TrimAll(value); }
    }

    #endregion Public properties

    #region Public methods

    public ProductGroup CreateChild() {
      ProductGroup child = new ProductGroup();
      child.Parent = this;

      return child;
    }

    public void RemoveChild(ProductGroup child) {
      ProductGroup readedChild = this.GetChild(child);

      readedChild.Status = GeneralObjectStatus.Deleted;
      readedChild.Save();
    }

    public void RemoveRule(ProductGroupRule rule) {
      ProductGroupRule readedRule = this.GetRule(rule);

      readedRule.Status = GeneralObjectStatus.Deleted;
      readedRule.Save();
    }

    public ProductGroupRule CreateRule() {
      return new ProductGroupRule(this);
    }

    public ProductGroup GetChild(ProductGroup child) {
      ProductGroup readedChild = this.GetChilds().Find((x) => x.Equals(child));

      if (readedChild != null) {
        return readedChild;
      } else {
        throw new ProductManagementException(ProductManagementException.Msg.InvalidChildCategory, child.Id, this.Id);
      }
    }

    public ProductGroupRule GetRule(ProductGroupRule rule) {
      ProductGroupRule readedRule = this.GetRules().Find((x) => x.Equals(rule));

      if (readedRule != null) {
        return readedRule;
      } else {
        throw new ProductManagementException(ProductManagementException.Msg.InvalidCategoryRule, rule.Id, this.Id);
      }
    }

    public ObjectList<ProductGroup> GetChilds() {
      return ProductsData.GetProductGroupChilds(this);
    }

    public ObjectList<ProductGroupRule> GetRules() {
      return ProductsData.GetProductGroupRules(this);
    }

    public ObjectList<ProductGroup> GetSiblings() {
      return ProductsData.GetProductGroupChilds(this.Parent);
    }

    protected override void ImplementsLoadObjectData(DataRow row) {
      this.number = (string) row["ProductGroupNumber"];
      this.name = (string) row["ProductGroupName"];
      this.englishName = (string) row["ProductGroupEnglishName"];
      this.description = (string) row["ProductGroupDescription"];
      this.tags = (string) row["ProductGroupTags"];
      this.keywords = (string) row["ProductGroupKeywords"];
      this.manager = Contact.Parse((int) row["ManagerId"]);
      this.modifiedBy = Contact.Parse((int) row["ModifiedById"]);
      this.parentId = (int) row["ParentProductGroupId"];
      this.status = (GeneralObjectStatus) Convert.ToChar(row["ProductGroupStatus"]);
    }

    protected override void ImplementsSave() {
      this.keywords = EmpiriaString.BuildKeywords(this.Tags, this.Number, this.Name, this.EnglishName);
      this.modifiedBy = Contact.Parse(ExecutionServer.CurrentUserId);

      ProductsData.WriteProductGroup(this);
    }

    #endregion Public methods

  } // class ProductGroup

} // namespace Empiria.Products