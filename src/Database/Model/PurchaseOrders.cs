﻿using System;
using System.Collections.Generic;

namespace MyMvc.Database.Model
{
    public partial class PurchaseOrders
    {
        public PurchaseOrders()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLines>();
            StockItemTransactions = new HashSet<StockItemTransactions>();
            SupplierTransactions = new HashSet<SupplierTransactions>();
        }

        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public int DeliveryMethodId { get; set; }
        public int ContactPersonId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string SupplierReference { get; set; }
        public bool IsOrderFinalized { get; set; }
        public string Comments { get; set; }
        public string InternalComments { get; set; }
        public int LastEditedBy { get; set; }
        public DateTime LastEditedWhen { get; set; }

        public People ContactPerson { get; set; }
        public DeliveryMethods DeliveryMethod { get; set; }
        public People LastEditedByNavigation { get; set; }
        public Suppliers Supplier { get; set; }
        public ICollection<PurchaseOrderLines> PurchaseOrderLines { get; set; }
        public ICollection<StockItemTransactions> StockItemTransactions { get; set; }
        public ICollection<SupplierTransactions> SupplierTransactions { get; set; }
    }
}
