/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Treasury Management System        *
*  Namespace : Empiria.Treasury                                 Assembly : Empiria.Treasury.dll              *
*  Type      : CRPostingList                                    Pattern  : Empiria List Class                *
*  Version   : 6.0        Date: 23/Oct/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : List of CRPosting instances belonging to a cash register trasaction.                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Collections.Generic;
using System.Data;

namespace Empiria.Treasury {

  /// <summary>List of CRPosting instances belonging to a cash register trasaction.</summary>
  public class CRPostingList : FixedList<CRPosting> {

    #region Fields

    private decimal instrumentAmount = decimal.Zero;
    private decimal inputAmount = decimal.Zero;
    private decimal outputAmount = decimal.Zero;

    #endregion Fields

    #region Constructors and parsers

    public CRPostingList() {
      //no-op
    }

    public CRPostingList(int capacity) : base(capacity) {
      // no-op
    }

    public CRPostingList(List<CRPosting> list): base(list) {
      this.CalculateTotals();
    }

    public CRPostingList(Func<DataRow, CRPosting> parser, DataView view): base(parser, view) {
      this.CalculateTotals();
    }

    #endregion Constructors and parsers

    #region Public properties

    public decimal InstrumentAmount {
      get { return instrumentAmount; }
    }

    public decimal InputAmount {
      get { return inputAmount; }
    }

    public decimal OutputAmount {
      get { return outputAmount; }
    }

    #endregion Public properties

    #region Public methods

    protected internal new void Add(CRPosting item) {
      base.Add(item);
      this.CalculateTotals();
    }

    public override CRPosting this[int index] {
      get {
        return (CRPosting) base[index];
      }
    }

    public new bool Contains(CRPosting item) {
      return base.Contains(item);
    }

    public new bool Contains(Predicate<CRPosting> match) {
      CRPosting result = base.Find(match);

      return (result != null);
    }

    public override void CopyTo(CRPosting[] array, int index) {
      for (int i = index, j = Count; i < j; i++) {
        array.SetValue(base[i], i);
      }
    }

    public new CRPosting Find(Predicate<CRPosting> match) {
      return base.Find(match);
    }

    public new List<CRPosting> FindAll(Predicate<CRPosting> match) {
      return base.FindAll(match);
    }

    protected internal new bool Remove(CRPosting item) {
      bool result = base.Remove(item);

      this.CalculateTotals();

      return result;
    }

    public new void Sort(Comparison<CRPosting> comparison) {
      base.Sort(comparison);
    }

    #endregion Public methods

    #region Private methods

    private void CalculateTotals() {
      this.instrumentAmount = decimal.Zero;
      this.inputAmount = decimal.Zero;
      this.outputAmount = decimal.Zero;
      for (int i = 0; i < this.Count; i++) {
        this.instrumentAmount += this[i].InstrumentAmount;
        this.inputAmount += this[i].InputAmount;
        this.outputAmount += this[i].OutputAmount;
      }
      if (this.Count == 1) {
        this[0].Transaction.BaseInstrumentType = this[0].InstrumentType;
      } if (this.Count > 1) {
        this[0].Transaction.BaseInstrumentType = InstrumentType.Multiple;
      }
    }

    #endregion Private methods;

  } // class CRPostingList

} // namespace Empiria.Treasury
