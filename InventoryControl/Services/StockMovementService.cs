using InventoryControl.Data;
using InventoryControl.Enums;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class StockMovementService
{
    private readonly AppDbContext _context;
    public StockMovementService(AppDbContext context) => _context = context;

    public async Task<PaginatedResultResponseRequest<StockMovement>> GetAllProductStockMovement(int pageNumber,
        int pageSize, Guid productId, MovementType? type)
    {
        var query = _context.StockMovements.AsQueryable();
        var totalItems = await query.CountAsync();

        query = query
            .OrderByDescending(sm => sm.CreatedAt)
            .AsNoTracking()
            .Where(sm => sm.ProductId.Equals(productId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        if (type != null)
        {
            if (type.Value == MovementType.IN || type.Value == MovementType.OUT)
            {
                query = query.Where(sm => sm.MovementType == type);
            }
        }

        var stockMovements = await query.ToListAsync();
        return new PaginatedResultResponseRequest<StockMovement>
        {
            Items = stockMovements,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }

    public async Task<PaginatedResultResponseRequest<StockMovement>> GetAllStockMovement(int pageNumber,
        int pageSize, MovementType? type)
    {
        var query = _context.StockMovements.AsQueryable();
        var totalItems = await query.CountAsync();

        query = query
            .OrderByDescending(sm => sm.CreatedAt)
            .AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        if (type != null)
        {
            if (type.Value == MovementType.IN || type.Value == MovementType.OUT)
            {
                query = query.Where(sm => sm.MovementType == type);
            }
        }

        var stockMovements = await query.ToListAsync();
        return new PaginatedResultResponseRequest<StockMovement>
        {
            Items = stockMovements,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }


    public async Task<StockMovement> AddStockMovementToProduct(CreateStockMovementRequest request)
    {
        var stockMovement = new StockMovement
        {
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            MovementType = request.MovementType
        };
        await _context.StockMovements.AddAsync(stockMovement);
        await _context.SaveChangesAsync();
        return stockMovement;
    }
}