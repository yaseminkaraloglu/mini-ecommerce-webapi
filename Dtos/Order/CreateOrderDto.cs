namespace MyE_CommerceWebAPI.Dtos.Order
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
