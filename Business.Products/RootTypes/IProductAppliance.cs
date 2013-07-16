/* Empiria® Business Framework 2013 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : IProductAppliance                                Pattern  : Separated Interface               *
*  Date      : 25/Jun/2013                                      Version  : 5.1     License: CC BY-NC-SA 3.0  *
*                                                                                                            *
*  Summary   : Interface that represents a product appliance.                                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2013. **/

namespace Empiria.Products {

  /// <summary>Interface that represents a product appliance.</summary>
  public interface IProductAppliance : IIdentifiable {

    #region Members definition

    string Name { get; }

    #endregion Members definition

  } // interface IProductAppliance

} // namespace Empiria.Products