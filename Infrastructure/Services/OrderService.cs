using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly StoreContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork,StoreContext context )
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
            _context = context;


        }
        //public async Task<Inventory> CreatedespatchAsync(string customerName,string order)
        //{
        //    var order =await 
        //}
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // get items from the product repo & checking for available quantity
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            { 
               var orders = await _unitOfWork.Repository<Order>().GetByIdAsync(item.Id);
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                //var inv = await _unitOfWork.Repository<Inventory>().GetByIdAsync(item.Id);
              
                if (item.Quantity > productItem.AvailableQuantity)
                {
                    return new Order { FailMessage = "Order can not be placed, any of the item's quantity in not available" };
                }
                if (item.Quantity > productItem.Limit)
                {
                    return new Order { FailMessage = "Only " + productItem.Limit + " items of " + productItem.Name + " can be placed for order at a time " };
                }

                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl, productItem.Description, productItem.Supplier);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                productItem.AvailableQuantity = productItem.AvailableQuantity - item.Quantity;
                productItem.QuAccept = productItem.QuAccept + item.Quantity;
                //productItem.QuReject += inv.TotalQuantity;


                _unitOfWork.Repository<Product>().Update(productItem);
                items.Add(orderItem);
            }

            // get delivery method from the repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calc subtotal
            var subtotal = items.Sum(i => i.Price * i.Quantity);

            // create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);
            _unitOfWork.Repository<Order>().Add(order);
            //  save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                return null;
            }
            // delete basket
            await _basketRepo.DeleteBasketAsync(basketId);
            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail); 

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail); 
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
       
    }
}