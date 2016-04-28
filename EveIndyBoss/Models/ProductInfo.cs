namespace EveIndyBoss.Models
{
    public class ProductInfo
    {
        public decimal JitaSell { get; set; }
        public decimal ShippingToStaging { get; set; }
        public decimal StagingSell { get; set; }
        public decimal SellAtProfit { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalCostPerItem { get; set; }
    }
}