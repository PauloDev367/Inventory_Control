using InventoryControl.Data;
using InventoryControl.Enums;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class SaleService
{
    private readonly AppDbContext _context;
    private readonly ProductService _productService;

    public SaleService(AppDbContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }

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
        
        await _productService.RemoverOrAddProductQuantityAsync(request.ProductId, request.Quantity, MovementType.OUT);
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
    
    public async Task<PaginatedResultResponseRequest<Sale>> GetAllSalesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Sales.AsQueryable();
        var totalItems = await query.CountAsync();

        var sales = await query
            .OrderByDescending(s => s.CreatedAt)
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