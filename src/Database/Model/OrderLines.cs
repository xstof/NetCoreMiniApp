﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Database.Model
{
    public partial class OrderLines
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int StockItemId { get; set; }
        public string Description { get; set; }
        public int PackageTypeId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
        public decimal TaxRate { get; set; }
        public int PickedQuantity { get; set; }
        public DateTime? PickingCompletedWhen { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }

        public People LastEditedByNavigation { get; set; }
        public Orders Order { get; set; }
        public PackageTypes PackageType { get; set; }
        public StockItems StockItem { get; set; }
    }
}
