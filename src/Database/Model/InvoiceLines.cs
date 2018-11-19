using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Database.Model
{
    public partial class InvoiceLines
    {
        public int InvoiceLineId { get; set; }
        public int InvoiceId { get; set; }
        public int StockItemId { get; set; }
        public string Description { get; set; }
        public int PackageTypeId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineProfit { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExtendedPrice { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }

        public Invoices Invoice { get; set; }
        public People LastEditedByNavigation { get; set; }
        public PackageTypes PackageType { get; set; }
        public StockItems StockItem { get; set; }
    }
}
