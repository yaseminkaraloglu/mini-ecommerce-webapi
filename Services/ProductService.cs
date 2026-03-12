using Microsoft.EntityFrameworkCore;
using MyE_CommerceWebAPI.Data;
using MyE_CommerceWebAPI.Dtos.Product;
using MyE_CommerceWebAPI.Models;
using MyE_CommerceWebAPI.Services.Interfaces;

namespace MyE_CommerceWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllAsync(bool onlyActive = true)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (onlyActive)
            {
                query = query.Where(p => p.IsActive);
            }
            return await query
                .OrderBy(p => p.Name)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsActive = p.IsActive,
                    CreatedDate = p.CreatedDate,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.Name : ""
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);


            if (p == null) return null;


            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : ""
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
      
            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Name boş olamaz.");


            if (dto.Price <= 0)
                throw new InvalidOperationException("Price 0'dan büyük olmalı.");


            if (dto.Stock < 0)
                throw new InvalidOperationException("Stock negatif olamaz.");


            var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);


            if (category == null)
                throw new InvalidOperationException("CategoryId geçersiz. Böyle bir kategori yok.");


            if (!category.IsActive)
                throw new InvalidOperationException("Bu kategori pasif. Bu kategoriye ürün ekleyemezsin.");


            var entity = new Product
            {
                Name = name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CategoryId = dto.CategoryId
            };


            _context.Products.Add(entity);
            await _context.SaveChangesAsync();


            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CategoryId = entity.CategoryId,
                CategoryName = category.Name
            };
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
          
            var entity = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);


            if (entity == null) return null;


        
            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("Name boş olamaz.");


            if (dto.Price <= 0)
                throw new InvalidOperationException("Price 0'dan büyük olmalı.");


            if (dto.Stock < 0)
                throw new InvalidOperationException("Stock negatif olamaz.");


            if (entity.CategoryId != dto.CategoryId)
            {
                var newCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);


                if (newCategory == null)
                    throw new InvalidOperationException("CategoryId geçersiz. Böyle bir kategori yok.");


                if (!newCategory.IsActive)
                    throw new InvalidOperationException("Bu kategori pasif. Ürünü bu kategoriye taşıyamazsın.");


                entity.CategoryId = dto.CategoryId;
                entity.Category = newCategory; 
            }


            entity.Name = name;
            entity.Description = dto.Description;
            entity.Price = dto.Price;
            entity.Stock = dto.Stock;
            entity.IsActive = dto.IsActive;


            await _context.SaveChangesAsync();


            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category != null ? entity.Category.Name : ""
            };
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return false;


            entity.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
