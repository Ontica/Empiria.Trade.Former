/* Empiria Business Framework 2014 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Business Framework                       System   : Product Data Management           *
*  Namespace : Empiria.Products                                 Assembly : Empiria.Products.dll              *
*  Type      : IProductAppliance                                Pattern  : Loose coupling interface          *
*  Version   : 5.5        Date: 25/Jun/2014                     License  : GNU AGPLv3  (See license.txt)     *
*                                                                                                            *
*  Summary   : Interface that represents a product appliance.                                                *
*                                                                                                            *
********************************* Copyright (c) 2002-2014. La Vía Óntica SC, Ontica LLC and contributors.  **/

namespace Empiria.Products {

  /// <summary>Interface that represents a product appliance.</summary>
  public interface IProductAppliance : IIdentifiable {

    #region Members definition

    string Name { get; }

    #endregion Members definition

  } // interface IProductAppliance

} // namespace Empiria.Products
