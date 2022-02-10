using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }
        public int AvailableQuantity { get; set; }
        public int Limit { get; set; }
        public string PictureUrl { get; set; } 

        public ProductType ProductType { get; set; }
        [ForeignKey(nameof(ProductType))]
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        [ForeignKey(nameof(ProductBrand))]
        public int ProductBrandId { get; set; }
        public int  QuAccept { get; set; }
        public int QuReject { get; set; }

    }
}