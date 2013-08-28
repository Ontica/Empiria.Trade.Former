/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : Service                                          Pattern  : Storage Item                      *
*  Date      : 23/Oct/2013                                      Version  : 5.2     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Represents a labour.                                                                          *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

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