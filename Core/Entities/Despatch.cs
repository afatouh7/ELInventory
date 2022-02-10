using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Despatch:BaseEntity
    {
       
      
        public CustomerName  CustomerName { get; set; }
        public OrderItem   OrderItem { get; set; }
        public int TotalQuantity { get; set; }
        public int DespatchQuantity { get; set; }
        public int PhysicalQuantity { get; set; }
        public string Technition { get; set; }
        public string ApprovelName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
       


    }
}
