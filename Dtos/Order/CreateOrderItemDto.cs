namespace MyE_CommerceWebAPI.Dtos.Order
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
