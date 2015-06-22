/* Empiria Trade 2015 ****************************************************************************************
*                                                                                                            *
*  Solution  : Empiria Trade                                    System   : Ordering System                   *
*  Namespace : Empiria.Trade.Ordering                           Assembly : Empiria.Trade.Ordering.dll        *
*  Type      : SupplyOrderItemList                              Pattern  : Empiria List Class                *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a list of BaseObject instances.                                                    *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

using Empiria.Products;

namespace Empiria.Trade.Ordering {

  /// <summary>Represents a list of BaseObject instances.</summary>
  public class SupplyOrderItemList : FixedList<SupplyOrderItem> {

    #region Fields

    private decimal repositionCost = decimal.Zero;
    private decimal productSubTotal = decimal.Zero;
    private decimal productDiscount = decimal.Zero;
    private decimal productTaxes = decimal.Zero;
    private decimal productTotal = decimal.Zero;
    private decimal shippingSubTotal = decimal.Zero;
    private decimal shippingDiscount = decimal.Zero;
    private decimal shippingTaxes = decimal.Zero;
    private decimal shippingTotal = decimal.Zero;

    #endregion Fields

    #region Constructors and parsers

    public SupplyOrderItemList() {
      //no-op
    }

    public SupplyOrderItemList(int capacity) : base(capacity) {
      // no-op
    }

    public SupplyOrderItemList(Func<DataRow, SupplyOrderItem> parser, DataView view) : base(parser, view) {
      this.CalculateTotals();
    }

    #endregion Constructors and parsers

    #region Public properties

    public decimal RepositionCost {
      get { return repositionCost; }
    }

    public decimal ProductSubTotal {
      get { return productSubTotal; }
    }

    public decimal ProductDiscount {
      get { return productDiscount; }
    }

    public decimal ProductSubTotalBeforeTaxes {
      get { return productSubTotal - productDiscount; }
    }

    public decimal ProductTaxes {
      get { return productTaxes; }
    }
    public decimal ProductTotal {
      get { return productTotal; }
    }
    public decimal ShippingSubTotal {
      get { return shippingSubTotal; }
    }
    public decimal ShippingDiscount {
      get { return shippingDiscount; }
    }
    public decimal ShippingTaxes {
      get { return shippingTaxes; }
    }

    public decimal ShippingTotal {
      get { return shippingTotal; }
    }

    public decimal SubTotal {
      get { return productSubTotal + shippingSubTotal; }
    }

    public decimal Discount {
      get { return productDiscount + shippingDiscount; }
    }

    public decimal TaxesTotal {
      get { return productTaxes + shippingTaxes; }
    }

    public decimal Total {
      get { return productTotal + shippingTotal; }
    }

    #endregion Public properties

    #region Public methods

    protected internal new void Add(SupplyOrderItem item) {
      base.Add(item);
      this.CalculateTotals();
    }

    public override SupplyOrderItem this[int index] {
      get {
        return (SupplyOrderItem) base[index];
      }
    }

    public bool Contains(Product product) {
      return base.Contains((x) => x.Product.Equals(product));
    }

    public new bool Contains(SupplyOrderItem item) {
      return base.Contains(item);
    }

    public new bool Contains(Predicate<SupplyOrderItem> match) {
      SupplyOrderItem result = base.Find(match);

      return (result != null);
    }

    public override void CopyTo(SupplyOrderItem[] array, int index) {
      for (int i = index, j = Count; i < j; i++) {
        array.SetValue(base[i], i);
      }
    }

    public SupplyOrderItem Find(Product product) {
      return base.Find((x) => x.Product.Equals(product));
    }

    public new SupplyOrderItem Find(Predicate<SupplyOrderItem> match) {
      return base.Find(match);
    }

    public new List<SupplyOrderItem> FindAll(Predicate<SupplyOrderItem> match) {
      return base.FindAll(match);
    }

    protected internal new bool Remove(SupplyOrderItem item) {
      bool result = base.Remove(item);

      this.CalculateTotals();

      return result;
    }

    public new void Sort(Comparison<SupplyOrderItem> comparison) {
      base.Sort(comparison);
    }

    #endregion Public methods

    #region Private methods

    private void CalculateTotals() {
      this.repositionCost = decimal.Zero;
      this.productSubTotal = decimal.Zero;
      this.productDiscount = decimal.Zero;
      this.productTaxes = decimal.Zero;
      this.productTotal = decimal.Zero;
      this.shippingSubTotal = decimal.Zero;
      this.shippingDiscount = decimal.Zero;
      this.shippingTaxes = decimal.Zero;
      this.shippingTotal = decimal.Zero;

      for (int i = 0; i < base.Count; i++) {
        this.repositionCost += base[i].RepositionValue;
        this.productSubTotal += base[i].ProductSubTotal;
        this.productDiscount += base[i].ProductDiscount;
        this.productTaxes += base[i].ProductTaxes;
        this.productTotal += base[i].ProductTotal;
        this.shippingSubTotal += base[i].ShippingSubTotal;
        this.shippingDiscount += base[i].ShippingDiscount;
        this.shippingTaxes += base[i].ShippingTaxes;
        this.shippingTotal += base[i].ShippingTotal;
      }
    }

    #endregion Private methods;

  } // class SupplyOrderItemList

} // namespace Empiria.Trade.Ordering
