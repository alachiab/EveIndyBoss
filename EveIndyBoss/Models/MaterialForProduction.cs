namespace EveIndyBoss.Models
{
    public class MaterialForProduction
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Volume { get; set; }
        public decimal CostAll { get; set; }
        public decimal VolumeAll { get; set; }
        public decimal Shipping { get; set; }
        public decimal Collateral { get; set; }
        public decimal CostTotal { get; set; }
    }
}