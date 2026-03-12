using MyE_CommerceWebAPI.Dtos.Product;

namespace MyE_CommerceWebAPI.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetAllAsync(bool onlyActive = true);
        public Task<ProductDto?> GetByIdAsync(int id);
        public Task<ProductDto> CreateAsync(CreateProductDto dto);
        public Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
        public Task<bool> SoftDeleteAsync(int id);
    }
}
