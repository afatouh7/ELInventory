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
    public class CustomerNameController : ControllerBase
    {
        private readonly StoreContext _context;

        public CustomerNameController(StoreContext context)
        {
            _context = context;
        } 
        [HttpGet] 
        public async Task<ActionResult> GetCutomerName()
        {
            var customer = await _context.CustomerNames.ToListAsync();
            return Ok(customer); 
        } 

        [HttpPost("PostCutomerName")]
        public async Task<ActionResult> PostCutomerName(CustomerName customerName)
        {
            var customer = await _context.CustomerNames.AddAsync(customerName); 
            _context.SaveChanges();
            return Ok(customer);
        
        }

    }
}
