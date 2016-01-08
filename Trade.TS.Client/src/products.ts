/**
 *  Solution : Empiria Trade Client                             || v0.1.0104
 *  Module   : Empiria.Trade.Products
 *  Summary  : Gets data and performs operations over products.
 *
 *  Author   : José Manuel Cota <https://github.com/jmcota>
 *  License  : GNU GPLv3. Other licensing terms are available. See <https://github.com/Ontica/Empiria.Client>
 *
 *  Copyright (c) 2015-2016. Ontica LLC, La Vía Óntica SC and contributors. <http://ontica.org>
*/

module Empiria.Trade.Products {

  /** Holds product data. */
  export interface ProductData {
    id: number;
    partNumber: string;
    name: string;
    brand: string;
    specification: string;
  }  // interface ProductData

  /** Type to handle products. */
  export class Product {

    // #region Fields

    private _data: ProductData = Product.empty;

    get data(): ProductData {
      return this._data;
    }

    // #endregion Fields

    // #region Constructor and parsers

    constructor(data: ProductData) {
      this._data = data;
    }

    /** Gets the empty instance for ProductData*/
    static empty: ProductData = {
      id: -1,
      partNumber: "",
      name: "",
      brand: "",
      specification: ""
    };

    /**
      * Static method that parses an existing product given its unique id.
      * @param id The product integer unique id.
      */
    static parse(id: number): Product {
      var dataOperation = Empiria.DataOperation.parse("getTradeProduct", id.toString());

      var data = dataOperation.getData();

      //Empiria.Assertion.hasValue(data, "There was a problem reading data for product {0}.", id);

      return new Product(this.prototype.convertToProductData(data));
    }

    // #endregion Constructor and parsers

    // #region Public methods

    /** Add an equivalent product to this product.
      * @param equivalentProduct The equivalent product of this product.
      */
    public addEquivalent(equivalentProduct: ProductData): Product {
      var dataOperation = Empiria.DataOperation.parse("apdTradeEquivalentProduct");

      var responseData = dataOperation.writeData(equivalentProduct);

      //Empiria.Assertion.hasValue(responseData, "There was a problem writing data for the equivalent product.");

      return new Product(this.convertToProductData(responseData));
    }

    // #endregion Public methods

    // #region Private methods

    /** Helper that converts a server Product object to a client ProductData type.*/
    private convertToProductData(serverData: any): ProductData {
      var productData = {
        id: serverData.id,
        partNumber: serverData.partNumber,
        name: serverData.name,
        brand: serverData.brand,
        specification: serverData.specification
      };

      return productData;
    }

    // #endregion Private methods

  }  // class Product

}  // module Empiria.Trade.Products
