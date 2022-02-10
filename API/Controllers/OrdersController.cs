 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using System.Net.Http;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    // [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public StoreContext _context { get; }

        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper, StoreContext context
            , IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _mapper = mapper;
            _orderService = orderService;
            _configuration = configuration;
            _context = context;
            _env = env;
        }
        //Create new order
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrncipal();
            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order.FailMessage != null)
            {
                return BadRequest(new ApiResponse(400, order.FailMessage));
            }
            if (order == null)
            {
                return BadRequest(new ApiResponse(400, "Problem creating oder"));
            }

            return Ok(order);
        }
        //order for user
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromPrncipal();
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        //All order in the stock
        [Authorize(Roles = "Stock")]
        [HttpGet("GetCompletOrdersForUser")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetCompletOrdersForUser()
        {
            //var email = HttpContext.User.RetrieveEmailFromPrncipal();
            //  var orders = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'PendingRecive' and BuyerEmail='" + email+"'").ToListAsync();
            //var filepath = _configuration.GetValue<string>("physicalpath");
           
            var orders = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'PendingRecive'").Include(x => x.OrderItems).ToListAsync(); 
            
            // var orders = await _orderService.GetOrdersForUserAsync(email);
            //if (orders != null) 
            //{
            //    var ord = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'PendingRecive' ").ToListAsync();
            //    return Ok(ord);

            //}

            return Ok(orders);

            //return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        //By id  
        [Authorize(Roles = "Stock")]
        [Authorize(Roles = "Finance")]
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrncipal();
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(_mapper.Map<Order, OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }

        [HttpPost("DeliveryMethod")]
        public async Task<IActionResult> DeliveryMethod(DeliveryMethod delivery)

        {
            var method = await _context.DeliveryMethods.AddAsync(delivery);
            _context.SaveChanges();
            return Ok(method);
                

        }

        //admin for check order
        [Authorize(Roles = "Admin")]
       
        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetAllOrders()
        {
            var orders = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'Pending' ")
                .Include(x=>x.OrderItems).ToListAsync();
            //var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }
         
         //finance
        [Authorize(Roles = "Finance")]
        [HttpGet("GetAllfinance")]
        public async Task<ActionResult<List<Order>>> GetAllfinance(string order)
        {
            var orders = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'PaymentReceived' ").Include(x => x.OrderItems).ToListAsync();
            // return await _orderService.GetAllOrder();
            //var orders = await _context.Orders.ToListAsync();
            return Ok(orders);
        }
      //  [Authorize(Roles = "Stock")]
        //[HttpGet("GetAllPendingRecivedOrder")]
        //public async Task<ActionResult<List<Order>>> GetAllPendingRecivedOrder(string order)
        //{
        //    var orders = await _context.Orders.FromSqlRaw("select * from Orders  where Status = 'PendingRecive' ").ToListAsync();
        //    // return await _orderService.GetAllOrder();
        //    //var orders = await _context.Orders.ToListAsync();
        //    return Ok(orders);
        //}

        //checng status
        [HttpPut("Edit")]
        public async Task<ActionResult<Order>> Edit(int id, OrderStatus orderStauts)
        {
            var orderindb = await _context.Orders.SingleOrDefaultAsync(x => x.Id == id);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
           // var order = await _context.Orders.SingleOrDefaultAsync(x=>x.Id==id); 
            
            orderindb.Status = orderStauts;
            _context.Orders.Update(orderindb);
            await _context.SaveChangesAsync(); 


            if (orderStauts == OrderStatus.PendingRecive)
            {

                var products = _context.Products.ToList(); 
                List<Product> products1= new List<Product>();
                var viewdata = _context.InventoryViews.ToList();
                var inv = await _context.Inventories.ToListAsync();
                List<Inventory> invlist = new List<Inventory>();
               
                foreach (var item in viewdata)
                {
                   
                    invlist.Add(new Inventory
                    {
                        OrderName = item.Name,
                        Supplier = item.Supplier,
                        TimeStamp = DateTime.Now,
                        TotalQuantity = item.Quantity, 

                    }); 
                  
                    
                }
                _context.Inventories.RemoveRange(inv);
                _context.SaveChanges();
                _context.Inventories.AddRange(invlist);
                _context.SaveChanges();
               // product.QuAccept = inv.TotalQuantity;

            }

            return Ok(orderindb);
        }


        
        //get bill
        [HttpGet("GetPhoto")]
        public async Task<Order> GetPhoto([FromRoute] int id)
        {
              var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order == null)
            {
                return null; 
            }
           
            return order;
            
        }
        //[HttpPut("saveFile")]
        //public async Task<IActionResult> SaveFile([FromForm] int id, Order  order)
        //{
        //    order.PictureUrl = await SaveIamge(order.PictureFile);
        //    var orderImg = await _context.Orders.SingleOrDefaultAsync(x => x.Id == id);
        //    _context.Orders.Update(orderImg); 
        //        _context.SaveChanges();

        //    return Ok(orderImg);

        //}
        // upload image(bill) for order 
        //public int? MyID { get; set; }
        //[BindProperty]
        //public IFormFile file { get; set; }
        //[BindProperty]
        //public int? ID { get; set; }

        //public void OnGet(int? id)
        //{
        //    MyID = id;
        //}
        [HttpPut("SaveFiles")]
        public async Task<IActionResult> SaveFiles([FromForm] int id, IFormFile file)
            {

            //var email = HttpContext.User.RetrieveEmailFromPrncipal();
            //var order = _orderService.GetOrderByIdAsync(id, email);
            if (file != null)
            { 
                if (file.Length > 0)
            {
                var myInv = _context.Orders.FirstOrDefault(x => x.Id == id);
                using (var target = new MemoryStream())
                {
                    file.CopyTo(target);
                    myInv.PictureUrl = target.ToArray();
                }
                _context.Orders.Update(myInv);
                await _context.SaveChangesAsync();

                }
                return Ok();
            }
            else 
            {
                return NoContent();
            }

            return BadRequest();

        }

        //try
        //{

        //    var pic = _context.Orders.SingleOrDefault(x => x.Id == id);
        //    var httpRequest = Request.Form;
        //    var posttedFile = httpRequest.Files[0];
        //    string filename = posttedFile.FileName;
        //    //var physicalpath = _env.ContentRootPath + "/wwwroot/images/Photos" + filename;
        //     var physicalpath = _env.WebRootPath + "/images/" + file.FileName;
        //    //var physicalpath = Path.Combine(@"F:\ElPAy\e-com-master\Angular\src\assets\images", file.FileName);
        //    using (var stream = new FileStream(physicalpath, FileMode.Create))
        //    {
        //        await posttedFile.CopyToAsync(stream);
        //    }
        //    pic.PictureUrl = physicalpath.ToString();


        //    _context.Orders.Update(pic);
        //    _context.SaveChanges();

        //    return Ok(physicalpath); 



        //}
        //catch (System.Exception)
        //{

        //    return Ok("anonymous.png ");
        //}

        [HttpPost("inventory")]
        public async Task<IActionResult> Inventory(Order order)
        {
            if (ModelState.IsValid)
            {
                var orders = await _context.Orders.FromSqlRaw("insert into Inventory (OrderName,sum(TotalQuantity)) select ItemOrdered_ProductName,Quantity from OrderItem ").ToListAsync();
                return Ok(orders);
            } 
            return NotFound();

        }

        //not need now 
        [NonAction]
        public async Task<string> SaveIamge(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_env.ContentRootPath, "Image", imageName);
            using (var fileStream=new FileStream(imagePath,FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStream);
            }
            return imageName;
        }
    }

}
