using Microsoft.EntityFrameworkCore;
using MyE_CommerceWebAPI.Data;
using MyE_CommerceWebAPI.Dtos.Category;
using MyE_CommerceWebAPI.Models;
using MyE_CommerceWebAPI.Services.Interfaces;

namespace MyE_CommerceWebAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllAsync(bool onlyActive = true)
        {
            var query = _context.Categories.AsQueryable();

            if (onlyActive)
                query = query.Where(c => c.IsActive);

            return await query
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,

                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var c = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (c == null)
                return null;

            return new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                CreatedDate = c.CreatedDate,
            };
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var name = dto.Name.Trim();

            var exists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());

            if (exists)
            {
                throw new InvalidOperationException("Bu isimde bir kategori zaten var.");
            }

            var entity = new Category
            {
                Name = name,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
            };
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null) { return null; }

            var name = dto.Name.Trim();

            var nameTaken = await _context.Categories
                .AnyAsync(c => c.Id != id && c.Name.ToLower() == name.ToLower());
            if (nameTaken)
            {
                throw new InvalidOperationException("Bu isimde başka bir kategori zaten var.");
            }

            entity.Name = name;
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
            };
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (entity == null) return false;


            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
