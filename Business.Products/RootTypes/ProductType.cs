/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : PhysicalProduct                                  Pattern  : Power type                        *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Power type that classifies product types.                                                     *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/
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