namespace Bosco.Core.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal ListingPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal WholesalerPrice { get; set; }
        public int Stock { get; set; }
        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
        public ProductModel()
        {
            Code = string.Empty;
            Description = string.Empty;
            Category = new();
            Brand = new();
        }
    }
}
