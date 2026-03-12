using MyE_CommerceWebAPI.Dtos.Order;

namespace MyE_CommerceWebAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto?> GetByIdAsync(int id);
    }
}