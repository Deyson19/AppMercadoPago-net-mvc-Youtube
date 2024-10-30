namespace AppMercadoPago_Youtube.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Unit { get; set; }
        public double UnitPrice { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
