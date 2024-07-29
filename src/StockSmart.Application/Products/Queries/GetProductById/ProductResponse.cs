namespace StockSmart.Application.Products.Queries.GetProductById
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string StatusName { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Size { get; set; }
    }
}
