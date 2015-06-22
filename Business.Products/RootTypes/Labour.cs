﻿/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Service                                          Pattern  : Storage Item                      *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a labour.                                                                          *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  /// <summary> Represents a labour.</summary>
  public class Labour : GeneralObject {

    #region Constructors and parsers

    private Labour() {
      // Required by Empiria Framework.
    }

    static public Labour Parse(int id) {
      return BaseObject.ParseId<Labour>(id);
    }

    static public Labour Empty {
      get { return BaseObject.ParseEmpty<Labour>(); }
    }

    static public Labour Unknown {
      get { return BaseObject.ParseUnknown<Labour>(); }
    }

    #endregion Constructors and parsers

  } // class Labour

} // namespace Empiria.Products
