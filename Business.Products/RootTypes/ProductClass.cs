/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductClass                                     Pattern  : Storage Item                      *
*  Version   : 5.5        Date: 28/Mar/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Abstract type that represents a physical good or service classificator.                       *
*                                                                                                            *
********************************* Copyright (c) 1999-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;

namespace Empiria.Products {

  /// <summary>Abstract type that represents a physical good or service classificator.</summary>
  public abstract class ProductClass : BaseObject {

    #region Fields

    private const string thisTypeName = "ObjectType.ProductClass";

    private string name = String.Empty;
    private string commonName = String.Empty;
    private string englishName = String.Empty;
    private string oldName = String.Empty;
    private string tags = String.Empty;
    private string description = String.Empty;
    private string observations = String.Empty;
    private string keywords = String.Empty;
    private Contact manager = Contact.Parse(ExecutionServer.OrganizationId);
    private Contact modifiedBy = Contact.Parse(ExecutionServer.OrganizationId);
    private DateTime modificationDate = DateTime.Today;
    private GeneralObjectStatus status = GeneralObjectStatus.Suspended;

    #endregion Fields

    #region Constructors and parsers

    protected ProductClass(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public ProductClass Parse(int id) {
      return BaseObject.Parse<ProductClass>(thisTypeName, id);
    }

    static internal ProductClass Parse(DataRow dataRow) {
      return BaseObject.Parse<ProductClass>(thisTypeName, dataRow);
    }

    static internal ProductClass Empty {
      get { return BaseObject.ParseEmpty<ProductClass>(thisTypeName); }
    }

    #endregion Constructors and parsers

    #region Public properties

    public string CommonName {
      get { return commonName; }
      set { commonName = EmpiriaString.TrimAll(value); }
    }

    public string Name {
      get { return name; }
      set { name = EmpiriaString.TrimAll(value); }
    }

    public string EnglishName {
      get { return englishName; }
      protected set { englishName = value; }
    }

    public string Description {
      get { return description; }
      set { description = value; }
    }

    public string Keywords {
      get { return keywords; }
      protected set { keywords = value; }
    }

    public Contact ModifiedBy {
      get { return modifiedBy; }
      set { modifiedBy = value; }
    }

    public DateTime ModificationDate {
      get { return modificationDate; }
      set { modificationDate = value; }
    }

    public string Observations {
      get { return observations; }
      set { observations = value; }
    }

    public string OldName {
      get { return oldName; }
      protected set { oldName = value; }
    }

    public Contact Manager {
      get { return manager; }
      set { manager = value; }
    }

    public GeneralObjectStatus Status {
      get { return status; }
      set { status = value; }
    }

    public string Tags {
      get { return tags; }
      set { tags = value; }
    }

    #endregion Public properties

  } // class ProductClass

} // namespace Empiria.Products