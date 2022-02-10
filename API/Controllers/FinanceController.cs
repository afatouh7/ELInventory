using API.Extensions;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StoreContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IOrderService _orderService;

        public FinanceController(IConfiguration configuration, StoreContext context,
            IWebHostEnvironment env,
            IOrderService orderService
            )
        {
            _configuration = configuration;
            _context = context;
            _env = env;
            _orderService = orderService;
        }
        [HttpGet] 
        public JsonResult Get()
        {
            var finance = _context.FinaceStorges.ToList();
          
            return new JsonResult(finance);
        } 
        [HttpPost] 
        public JsonResult post(finaceStorge finace,int id )
        {
            var email = HttpContext.User.RetrieveEmailFromPrncipal();
             var order =  _orderService.GetOrderByIdAsync(id, email);
            var Fin = _context.Add(finace);

            _context.SaveChanges();
            return new JsonResult("Added Successfuly");
        }
        //[Route("SaveFile")]
        [HttpPost("SaveFile")]
       
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var posttedFile = httpRequest.Files[0]; 
                string filename=posttedFile.FileName;
                var physicalpath = _env.ContentRootPath + "/Photos" + filename; 
                using(var stream=new FileStream(physicalpath,FileMode.Create))
                {
                    posttedFile.CopyTo(stream);
                }
                return new JsonResult(filename);

            }
            catch (System.Exception)
            {

                return new JsonResult("anonymous.png "); 
            }
        }

    }
}
