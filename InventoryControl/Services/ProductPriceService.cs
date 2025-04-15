using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class ProductPriceService
{
    private readonly AppDbContext _context;

    public ProductPriceService(AppDbContext context) => _context = context;
    public async Task<PaginatedResultResponseRequest<ProductPrice>> GetAllProductPriceFromProductAsync(int pageNumber, int pageSize, Guid productId)
    {
        var query = _context.ProductPrices.AsQueryable();
        var totalItems = await query.CountAsync();

        var prices = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.ProductId.Equals(productId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<ProductPrice>
        {
            Items = prices,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
    
    public async Task<ProductPrice> CreateProductPriceAsync(CreateProductPriceRequest request)
    {
        var productPrice = new ProductPrice()
        {
            ProductId = request.ProductId,
            Price = request.Price,
        };

        await _context.ProductPrices.AddAsync(productPrice);
        await _context.SaveChangesAsync();
        return productPrice;
    }

}