/* Empiria Extended Framework 2015 ***************************************************************************
*                                                                                                            *
*  Solution  : Empiria Extended Framework                       System   : Document Management Services      *
*  Namespace : Empiria.Products.Data                            Assembly : Empiria.Documents.dll             *
*  Type      : ProductsData                                     Pattern  : Data Services Static Class        *
*  Version   : 6.0        Date: 04/Jan/2015                     License  : Please read license.txt file      *
*                                                                                                            *
*  Summary   : Provides database read and write methods for product data management.                         *
*                                                                                                            *
********************************* Copyright (c) 2002-2015. La Vía Óntica SC, Ontica LLC and contributors.  **/
using System;
using System.Data;

using Empiria.Data;

namespace Empiria.Products.Data {

  /// <summary>Provides database read and write methods for product data management</summary>
  static public class ProductsData {

    #region Public methods

    static public FixedList<Product> GetActiveProducts(string keywords, string sort) {
      string sql = "SELECT * FROM vwPLMActiveProducts";
      if (!String.IsNullOrWhiteSpace(keywords)) {
        sql += " WHERE (" + SearchExpression.ParseAndLikeWithNoiseWords("ProductKeywords", keywords) + ")";
      }
      if (!String.IsNullOrWhiteSpace(sort)) {
        sql += " ORDER BY " + sort;
      }
      return DataReader.GetList<Product>(DataOperation.Parse(sql),
                                              (x) => BaseObject.ParseList<Product>(x)).ToFixedList();
    }

    static public FixedList<ProductGroup> GetProductGroups(string keywords) {
      string sql = "SELECT * FROM PLMProductGroups";

      string filter = SearchExpression.ParseAndLikeWithNoiseWords("ProductGroupKeywords", keywords);
      if (filter.Length != 0) {
        sql += " WHERE (" + filter + ") AND (ProductGroupStatus <> 'X')";
      } else {
        sql += " WHERE (ProductGroupStatus <> 'X')";
      }
      sql += " ORDER BY ProductGroupName";

      return DataReader.GetList<ProductGroup>(DataOperation.Parse(sql), 
                                              (x) => BaseObject.ParseList<ProductGroup>(x)).ToFixedList();
    }

    static public FixedList<ProductGroup> GetProductGroups(ProductClass productTerm) {
      string sql = "SELECT * FROM PLMProductGroups INNER JOIN PLMProductGroupRules ";
      sql += "ON PLMProductGroups.ProductGroupId = PLMProductGroupRules.ProductGroupId ";
      sql += "WHERE PLMProductGroupRules.ProductTypeId = " + productTerm.Id.ToString() + " AND ";
      sql += "PLMProductGroupRules.ProductGroupRuleStatus <> 'X' AND PLMProductGroups.ProductGroupStatus <> 'X' ";
      sql += "ORDER BY PLMProductGroups.ProductGroupName";

      return DataReader.GetList<ProductGroup>(DataOperation.Parse(sql),
                                              (x) => BaseObject.ParseList<ProductGroup>(x)).ToFixedList();

    }

    static public FixedList<ProductGroup> GetProductGroupChilds(ProductGroup parentGroup) {
      var operation = DataOperation.Parse("qryPLMProductGroupChilds", 1, parentGroup.Id);

      return DataReader.GetList<ProductGroup>(operation,
                                              (x) => BaseObject.ParseList<ProductGroup>(x)).ToFixedList();
    }

    static public FixedList<ProductGroupRule> GetProductGroupRules(ProductGroup productGroup) {
      var operation = DataOperation.Parse("qryPLMProductGroupRules", 1, productGroup.Id);

      var list = DataReader.GetList<ProductGroupRule>(operation,
                                                     (x) => BaseObject.ParseList<ProductGroupRule>(x));

      list.Sort((x, y) => x.ProductTerm.Name.CompareTo(y.ProductTerm.Name));

      return list.ToFixedList();
    }

    #endregion Public methods

    #region Internal methods

    static internal int WriteProduct(Product o) {
      var operation = DataOperation.Parse("writePLMProduct", o.Id, o.GetEmpiriaType().Id,
                      o.ProductTerm.Id, -1, -1, o.Manager.Id, o.IsService,
                      o.IsCompound, o.IsCustomizable, o.NeedsReview,
                      o.Manufacturer.Id, o.Brand.Id, o.OriginCountry.Id, o.Model, o.PartNumber, o.Name,
                      o.ImageFile, o.SmallImageFile, o.SearchTags, o.Specification, o.Notes, o.Keywords,
                      o.PresentationUnit.Id, o.ContentsQty, o.ContentsUnit.Id, (char) o.PackagingType,
                      (char) o.IdentificationLevel, o.BarCodeID, o.RadioFrequenceID, o.LengthSize,
                      o.WidthSize, o.HeightSize, o.SizeUnit.Id, o.Weight, o.WeightUnit.Id,
                      o.ReviewedBy.Id, o.PostedBy.Id, o.ReplacedById, (char) o.Status,
                      o.StartDate, o.EndDate, o.LegacyKey);
      return DataWriter.Execute(operation);
    }

    static internal int WriteProductGroup(ProductGroup o) {
      DataOperation dataOperation = DataOperation.Parse("writePLMProductGroup", o.Id, 1,
                            o.Number, o.Name, o.EnglishName, o.Description, o.Tags, o.Keywords,
                            o.Manager.Id, o.ModifiedBy.Id, o.Parent.Id, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    static internal int WriteProductGroupRule(ProductGroupRule o) {
      DataOperation dataOperation = DataOperation.Parse("writePLMProductGroupRule", o.Id, 1,
                                                        o.Group.Id, o.ProductTerm.Id, o.ProductPosition.Id,
                                                        o.PostedBy.Id, (char) o.Status);
      return DataWriter.Execute(dataOperation);
    }

    #endregion Internal methods

  } // class ProductsData

} // namespace Empiria.Products.Data
