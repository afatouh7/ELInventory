using Microsoft.AspNetCore.Http;

namespace API.Dtos
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
        public string PictureUrl { get; set; }
        public IFormFile Picture { get; set; }
    }
}