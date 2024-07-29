using System;

namespace StockSmart.Application.Dto
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime RetrievedAt { get; set; }

    }
}
