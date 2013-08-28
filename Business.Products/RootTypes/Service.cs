﻿/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Service                                          Pattern  : Storage Item                      *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a service.                                                                         *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.Products {

  public sealed class Service : Product {

    #region Fields

    private const string thisTypeName = "ObjectType.Product.Service";

    #endregion Fields

    #region Constructors and parsers

    public Service()
      : base(thisTypeName) {

    }

    private Service(string typeName)
      : base(typeName) {
      // Required by Empiria Framework. Do not delete. Protected in not sealed classes, private otherwise
    }

    static public new Service Parse(int id) {
      return BaseObject.Parse<Service>(thisTypeName, id);
    }

    #endregion Constructors and parsers

  } // class Service

} // namespace Empiria.Products