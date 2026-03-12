using MyE_CommerceWebAPI.Dtos.Category;

namespace MyE_CommerceWebAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllAsync(bool onlyActive = true);
        public Task<CategoryDto?> GetByIdAsync(int id);
        public Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        public Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto);
        public Task<bool> SoftDeleteAsync(int id);
    }
}
