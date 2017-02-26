/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : ProductType                                      Pattern  : Power type                        *
*  Version   : 2.2                                              License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Power type that describes products.                                                           *
*                                                                                                            *
********************************* Copyright (c) 2002-2017. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

using Empiria.Ontology;

namespace Empiria.Products {

  /// <summary>Power type that describes products.</summary>
  [Powertype(typeof(Product))]
  public sealed class ProductType : Powertype {

    #region Constructors and parsers

    private ProductType() {
      // Empiria powertype types always have this constructor.
    }

    static public new ProductType Parse(int typeId) {
      return ObjectTypeInfo.Parse<ProductType>(typeId);
    }

    static public new ProductType Parse(string typeName) {
      return ObjectTypeInfo.Parse<ProductType>(typeName);
    }

    static public ProductType Empty {
      get {
        return ProductType.Parse("ObjectType.Product");
      }
    }

    #endregion Constructors and parsers

  } // class ProductType

} // namespace Empiria.Products
