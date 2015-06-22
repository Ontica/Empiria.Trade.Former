/* Empiria Business Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Service                                          Pattern  : Storage Item                      *
*  Version   : 2.0        Date: 25/Jun/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Represents a service.                                                                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;

namespace Empiria.Products {

  public sealed class Service : Product {

    #region Constructors and parsers

    private Service(ProductType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }

    static public new Service Parse(int id) {
      return BaseObject.ParseId<Service>(id);
    }

    #endregion Constructors and parsers

  } // class Service

} // namespace Empiria.Products
