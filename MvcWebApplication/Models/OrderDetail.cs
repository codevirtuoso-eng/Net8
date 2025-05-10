namespace MvcWebApplication.Models
{
    public class OrderDetail
    {
        public string OrderDetailId { get; set; } // Added OrderDetailId property
        public string OrderId { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}