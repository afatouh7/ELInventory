using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class Inventory 
    {
        public Inventory()
        {

        }
        public Inventory(int inventoryId, string supplier, string orderName,int totalQuantity)
        {  
            InventoryId = inventoryId;
            Supplier = supplier;
            OrderName = orderName;
            TotalQuantity = totalQuantity;

        }
        public int InventoryId { get; set; }
        public string Supplier { get; set; }
        public string OrderName { get; set; }
        public int TotalQuantity { get; set; }
        public DateTime TimeStamp { get; set; }
      //  public inventoryTransaction  InventoryTransaction { get; set; }
        //[ForeignKey(nameof(InventoryTransaction))]
       // public int InventoryId { get; set; }
    }
}
