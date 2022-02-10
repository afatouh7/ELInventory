using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class inventoryTransaction:BaseEntity
    {
        public inventoryTransaction()
        {

        }
        public inventoryTransaction(int desQuantity,int physicalQuantity, Inventory inventory)
        {
            Inventory = inventory;
            DesQuantity =desQuantity; 
            PhysicalQuantity=physicalQuantity;
            
        }
       
        public int DesQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public DateTime ActionTime { get; set; }
        public Inventory Inventory   { get; set; }

    }
}
