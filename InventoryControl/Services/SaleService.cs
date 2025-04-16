using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class SaleService
{
    private readonly AppDbContext _context;

    public SaleService(AppDbContext context) => _context = context;

    public async Task<Sale> CreateAsync(CreateSaleRequest request)
    {
        var totalPrice = request.Quantity * request.UnityPrice;
        var sale = new Sale
        {
            ProductId = request.ProductId,
            TotalPrice = totalPrice,
            Quantity = request.Quantity,
            UnityPrice = request.UnityPrice,
        };

        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();

        return sale;
    }

    public async Task<PaginatedResultResponseRequest<Sale>> GetProductSalesAsync(int pageNumber, int pageSize,
        Guid productId)
    {
        var query = _context.Sales.AsQueryable();
        var totalItems = await query.CountAsync();

        var sales = await query
            .OrderByDescending(s => s.CreatedAt)
            .Where(s => s.ProductId == productId)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<Sale>
        {
            Items = sales,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}