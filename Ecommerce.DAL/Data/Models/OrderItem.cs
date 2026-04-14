namespace Ecommerce.DAL
{
    public class OrderItem : BaseEntity
    {
        // this class for the product at the time of the order, so we need to save the price and quantity at that time, not just reference to the product
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } 

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}