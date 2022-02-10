using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly StoreContext _context;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryController(StoreContext context, IConnectionMultiplexer redis,
             IGenericRepository<Product> productRepo, IUnitOfWork unitOfWork)
        {
            _context = context;
            _productRepo = productRepo;
            _unitOfWork = unitOfWork;
            _database = redis.GetDatabase();
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAllInventoryorder()
        {
            var inv = await _context.Inventories.ToListAsync();

            return Ok(inv);
        }
        //[HttpPut("Edit")]
        //public async Task<ActionResult<Despatch>> Edit(Despatch despatch)

        //{

        //    var inv = await _context.Inventories.SingleOrDefaultAsync();
        //    var prod = await _context.Products.SingleOrDefaultAsync(); 
        //    var customer=await _context.CustomerNames.SingleOrDefaultAsync();
        //    //var inv = await _context.Inventories.SingleOrDefaultAsync(); 

        //  if(despatch != null)
        //   {


        //   }
        //} 

        //[HttpPost]
        //public async Task<ActionResult<inventoryTransaction>> PostInventory(inventoryTransaction transaction)
        ////{
        ////    //var inv = await _context.Inventories.SingleOrDefaultAsync();
        ////    //var dat = await _context.Transactions.SingleOrDefaultAsync();
        ////    //dat.PhysicalQuantity = inv.TotalQuantity - dat.DesQuantity;
        ////    //_context.Transactions.Add(transaction);
        ////    //_context.SaveChanges();
        ////    //return Ok();  
        //    var trans = new List<inventoryTransaction>(); 
        //    foreach(OrderIte)
        ////    //var inv = await _unitOfWork.Repository<Inventory>().GetByIdAsync();
        //    var tansaction = new inventoryTransaction(desQuantity, inventory,physicalQuantity);

        //} 
        [HttpPost]
        public async Task<ActionResult<inventoryTransaction>> PostInventory(inventoryTransaction transaction)
        {
            var inventory = _context.Inventories.ToList();
           // var inv = new List<inventoryTransaction>();
            foreach (var item in inventory)
            {
                var invItem = _context.Inventories.Find(item.InventoryId);
                var Trans= _context.Transactions.Find(item.InventoryId);

                var invent = new Inventory(invItem.InventoryId,invItem.Supplier,invItem.OrderName ,invItem.TotalQuantity);
                var inventoryTransaction = new inventoryTransaction(transaction.DesQuantity,Trans.PhysicalQuantity,Trans.Inventory);
                var desk = transaction.DesQuantity + transaction.DesQuantity;
                transaction.PhysicalQuantity = invent.TotalQuantity - desk;
                
                
            } 
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return Ok(inventory);
        }



    }
}
