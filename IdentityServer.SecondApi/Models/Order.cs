namespace IdentityServer.SecondApi.Models
{
    public class Order
    {
        public object OrderId { get; set; }
        public object Item { get; set; }
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
        public string Price { get; set; }
    }
}