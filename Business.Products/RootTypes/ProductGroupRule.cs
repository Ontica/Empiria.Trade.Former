/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductGroupRule                                 Pattern  : Storage Item                      *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a product grouping category.                                                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;
using Empiria.Products.Data;

namespace Empiria.Products {

  /// <summary>Represents a product grouping category.</summary>
  public class ProductGroupRule : BaseObject {

    #region Fields

    private ProductGroup group = ProductGroup.Empty;
    private ProductClass productTerm = ProductClass.Empty;
    private ProductClass productPosition = ProductClass.Empty;
    private Contact postedBy = Person.Empty;
    private GeneralObjectStatus status = GeneralObjectStatus.Pending;

    #endregion Fields

    #region Constructors and parsers

    private ProductGroupRule() {
      // Required by Empiria Framework.
    }

    public ProductGroupRule(ProductGroup group) {
      this.group = group;
    }

    static public ProductGroupRule Empty {
      get { return BaseObject.ParseEmpty<ProductGroupRule>(); }
    }

    static public ProductGroupRule Parse(int id) {
      return BaseObject.ParseId<ProductGroupRule>(id);
    }

    #endregion Constructors and parsers

    #region Public properties

    public ProductClass ProductPosition {
      get { return productPosition; }
      set { productPosition = value; }
    }

    public ProductClass ProductTerm {
      get { return productTerm; }
      set { productTerm = value; }
    }

    public Contact PostedBy {
      get { return postedBy; }
      set { postedBy = value; }
    }

    public ProductGroup Group {
      get { return group; }
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
          case GeneralObjectStatus.Active:
            return "Activa";
          case GeneralObjectStatus.Suspended:
            return "Suspendida";
          case GeneralObjectStatus.Deleted:
            return "Eliminada";
          default:
            return "No determinado";
        }
      } // get
    }

    #endregion Public properties

    #region Public methods

    public FixedList<ProductGroup> GetProductGroups() {
      return ProductsData.GetProductGroups(this.ProductTerm);
    }

    protected override void OnLoadObjectData(DataRow row) {
      this.group = ProductGroup.Parse((int) row["ProductGroupId"]);
      this.productTerm = ProductClass.Parse((int) row["ProductTypeId"]);
      this.productPosition = ProductClass.Parse((int) row["ProductPositionId"]);
      this.postedBy = Contact.Parse((int) row["PostedById"]);
      this.status = (GeneralObjectStatus) Convert.ToChar(row["ProductGroupRuleStatus"]);
    }

    protected override void OnSave() {
      this.postedBy = Contact.Parse(ExecutionServer.CurrentUserId);

      ProductsData.WriteProductGroupRule(this);
    }

    #endregion Public methods

  } // class ProductGroup

} // namespace Empiria.Products
