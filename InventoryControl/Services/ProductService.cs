using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class ProductService
{
    private readonly AppDbContext _context;
    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResultResponseRequest<Product>> GetProducts(int pageNumber, int pageSize)
    {
        var query = _context.Products.AsQueryable();
        var totalItems= await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<Product>
        {
            Items = products,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
    
}