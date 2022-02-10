using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespatchController : ControllerBase
    {
        private readonly StoreContext _context;

        public DespatchController(StoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetInventoryData()

        {
            var inv = await _context.Inventories.ToListAsync();

            return Ok(inv);

        }
        //[HttpPost] 
        //public async Task<IActionResult> PostInventory(InventoryTransaction transaction)
        //{
        //   await _context.Transactions.AddAsync(transaction);
        //    _context.SaveChanges();
        //    return Ok();
        //}
    }
}
