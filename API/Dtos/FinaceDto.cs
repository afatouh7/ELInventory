using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Http;

namespace API.Dtos
{
    public class FinaceDto
    {
        public Order  Order { get; set; }
        public string BuyerEmail { get; set; }

        public string PhotoFileName { get; set; }
        public IFormFile Photo { get; set; }
    }
}
