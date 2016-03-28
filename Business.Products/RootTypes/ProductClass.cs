/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductClass                                     Pattern  : Storage Item                      *
*  Version   : 2.1                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Abstract type that represents a physical good or service classificator.                       *
*                                                                                                            *
********************************* Copyright (c) 2002-2016. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Contacts;

namespace Empiria.Products {

  /// <summary>Abstract type that represents a physical good or service classificator.</summary>
  public abstract class ProductClass : BaseObject {

    #region Fields

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

    protected ProductClass() {
      // Required by Empiria Framework.
    }

    static public ProductClass Parse(int id) {
      return BaseObject.ParseId<ProductClass>(id);
    }

    static internal ProductClass Empty {
      get { return BaseObject.ParseEmpty<ProductClass>(); }
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
