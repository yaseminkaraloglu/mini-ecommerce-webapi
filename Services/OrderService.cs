using Microsoft.EntityFrameworkCore;
using MyE_CommerceWebAPI.Data;
using MyE_CommerceWebAPI.Dtos.Order;
using MyE_CommerceWebAPI.Models;
using MyE_CommerceWebAPI.Services.Interfaces;

namespace MyE_CommerceWebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            if (dto == null)
                throw new InvalidOperationException("Request boş olamaz.");

            if (dto.Items == null || dto.Items.Count == 0)
                throw new InvalidOperationException("Sipariş için en az 1 ürün göndermelisin.");

            
            if (dto.Items.Any(i => i.Quantity <= 0))
                throw new InvalidOperationException("Quantity 0'dan büyük olmalı.");

           
            var mergedItems = dto.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new CreateOrderItemDto
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var productIds = mergedItems.Select(i => i.ProductId).Distinct().ToList();

            
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    Status = "Pending",
                    CreatedDate = DateTime.UtcNow
                };

                decimal total = 0m;

                foreach (var item in mergedItems)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                        throw new InvalidOperationException($"ProductId geçersiz: {item.ProductId}");

                    if (!product.IsActive)
                        throw new InvalidOperationException($"Ürün pasif: {product.Name}");

                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException(
                            $"Stok yetersiz: {product.Name}. Stok={product.Stock}, İstenen={item.Quantity}");

                    
                    product.Stock -= item.Quantity;

                    var unitPrice = product.Price;
                    var lineTotal = unitPrice * item.Quantity;

                    total += lineTotal;

                    order.Items.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        LineTotal = lineTotal
                    });
                }

                order.TotalAmount = total;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();

                
                return new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    CreatedDate = order.CreatedDate,
                    Items = order.Items.Select(oi =>
                    {
                        var p = products.First(x => x.Id == oi.ProductId);
                        return new OrderItemDto
                        {
                            ProductId = oi.ProductId,
                            ProductName = p.Name,
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice,
                            LineTotal = oi.LineTotal
                        };
                    }).ToList()
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            
            var productIds = order.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CreatedDate = order.CreatedDate,
                Items = order.Items.Select(oi =>
                {
                    var p = products.FirstOrDefault(x => x.Id == oi.ProductId);
                    return new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = p?.Name ?? "",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        LineTotal = oi.LineTotal
                    };
                }).ToList()
            };
        }
    }
}
