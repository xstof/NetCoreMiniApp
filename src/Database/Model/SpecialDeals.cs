using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Database.Model
{
    public partial class SpecialDeals
    {
        public int SpecialDealId { get; set; }
        public int? StockItemId { get; set; }
        public int? CustomerId { get; set; }
        public int? BuyingGroupId { get; set; }
        public int? CustomerCategoryId { get; set; }
        public int? StockGroupId { get; set; }
        public string DealDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
         [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
         [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }

        public BuyingGroups BuyingGroup { get; set; }
        public Customers Customer { get; set; }
        public CustomerCategories CustomerCategory { get; set; }
        public People LastEditedByNavigation { get; set; }
        public StockGroups StockGroup { get; set; }
        public StockItems StockItem { get; set; }
    }
}
