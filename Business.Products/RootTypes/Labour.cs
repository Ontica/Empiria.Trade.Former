﻿/* Empiria® Business Framework 2014 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Service                                          Pattern  : Storage Item                      *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Represents a labour.                                                                          *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.Products {

  public class Labour : GeneralObject {

    #region Fields

    private const string thisTypeName = "ObjectType.GeneralObject.Labour";

    #endregion Fields

    #region Constructors and parsers

    public Labour()
      : base(thisTypeName) {

    }

    protected Labour(string typeName)
      : base(typeName) {

    }

    static public Labour Parse(int id) {
      return BaseObject.Parse<Labour>(thisTypeName, id);
    }

    static public Labour Empty {
      get { return BaseObject.ParseEmpty<Labour>(thisTypeName); }
    }

    static public Labour Unknown {
      get { return BaseObject.ParseUnknown<Labour>(thisTypeName); }
    }

    #endregion Constructors and parsers

  } // class Labour

} // namespace Empiria.Products