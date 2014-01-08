/* Empiria® Business Framework 2014 **************************************************************************
*                                                                                                            *
*  Solution  : Empiria® Business Framework                      System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : IProductAppliance                                Pattern  : Separated Interface               *
*  Date      : 28/Mar/2014                                      Version  : 5.5     License: CC BY-NC-SA 4.0  *
*                                                                                                            *
*  Summary   : Interface that represents a product appliance.                                                *
*                                                                                                            *
**************************************************** Copyright © La Vía Óntica SC + Ontica LLC. 1999-2014. **/

namespace Empiria.Products {

  /// <summary>Interface that represents a product appliance.</summary>
  public interface IProductAppliance : IIdentifiable {

    #region Members definition

    string Name { get; }

    #endregion Members definition

  } // interface IProductAppliance

} // namespace Empiria.Products