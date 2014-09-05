/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : PhysicalProduct                                  Pattern  : Power type                        *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Power type that classifies product types.                                                     *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using Empiria.Ontology;

namespace Empiria.Products {

  /// <summary>Power type that classifies product types.</summary>
  public sealed class ProductType : PowerType<Product> {

    #region Fields

    private const string thisTypeName = "PowerType.ProductType";

    #endregion Fields

    #region Constructors and parsers

    private ProductType(int typeId)
      : base(thisTypeName, typeId) {
      // Empiria Power type pattern classes always has this constructor. Don't delete
    }

    static public new ProductType Parse(int typeId) {
      return PowerType<Product>.Parse<ProductType>(typeId);
    }

    static internal ProductType Parse(ObjectTypeInfo typeInfo) {
      return PowerType<Product>.Parse<ProductType>(typeInfo);
    }

    static public ProductType Empty {
      get {
        return ProductType.Parse(ObjectTypeInfo.Parse("ObjectType.Product.Empty"));
      }
    }

    #endregion Constructors and parsers

  } // class ProductType

} // namespace Empiria.Products
