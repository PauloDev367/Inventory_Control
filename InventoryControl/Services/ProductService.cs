using InventoryControl.Data;
using InventoryControl.Enums;
using InventoryControl.Handlers;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class ProductService
{
    private readonly AppDbContext _context;
    private readonly StockMovementService _stockMovementService;
    private readonly ProductPriceService _productPriceService;
    private readonly SendStockAlertHandler _sendStockAlertHandler;

    public ProductService(AppDbContext context, StockMovementService stockMovementService, ProductPriceService productPriceService, SendStockAlertHandler sendStockAlertHandler)
    {
        _context = context;
        _stockMovementService = stockMovementService;
        _productPriceService = productPriceService;
        _sendStockAlertHandler = sendStockAlertHandler;
    }

    public async Task<PaginatedResultResponseRequest<Product>> GetProductsAsync(int pageNumber, int pageSize)
    {
        var query = _context.Products.AsQueryable();
        var totalItems = await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
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

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            MinimumStock = request.MinimumStock,
        };

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var productPriceRequest = new CreateProductPriceRequest
        {
            ProductId = product.Id,
            Price = product.Price,
        };
        await _productPriceService.CreateProductPriceAsync(productPriceRequest);
        return product;
    }

    public async Task<Product> UpdateProductAsync(UpdateProductRequest request, Guid productId)
    {
        var product = await _context.Products
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(productId));

        if (product == null)
            throw new KeyNotFoundException();

        var priceBeforeUpdate = product.Price;
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.MinimumStock = request.MinimumStock;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        if (product.Price != priceBeforeUpdate)
        {
            var productPriceRequest = new CreateProductPriceRequest
            {
                ProductId = product.Id,
                Price = product.Price,
            };
            await _productPriceService.CreateProductPriceAsync(productPriceRequest);
        }
        return product;
    }

    public async Task<Product?> GetOneById(Guid productId)
    {
        return await _context.Products
            .Where(p => p.DeletedAt == null)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(productId));
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(productId));

        if (product == null)
            throw new KeyNotFoundException();

        product.DeletedAt = DateTime.Now;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task AddCategoryToProductAsync(Guid productId, AddCategoryToProductRequest request)
    {
        var product = await _context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == productId && p.DeletedAt == null);

        if (product == null)
            throw new KeyNotFoundException();

        var categories = await _context.Categories
            .Where(c => request.CategoriesId.Contains(c.Id) && c.DeletedAt == null)
            .ToListAsync();

        foreach (var category in categories)
        {
            if (!product.Categories.Contains(category))
                product.Categories.Add(category);
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveCategoryFromProductAsync(Guid productId, RemoveCategoryToProductRequest request)
    {
        var product = await _context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == productId && p.DeletedAt == null);

        if (product == null)
            throw new KeyNotFoundException();

        var categoriesToRemove = await _context.Categories
            .Where(c => request.CategoriesId.Contains(c.Id) && c.DeletedAt == null)
            .ToListAsync();

        foreach (var category in categoriesToRemove)
        {
            if (product.Categories.Contains(category))
                product.Categories.Remove(category);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Category>> GetAllProductCategoriesAsync(Guid productId)
    {
        var product = await _context.Products
            .Where(p => p.DeletedAt == null)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id.Equals(productId));

        if (product == null)
            throw new KeyNotFoundException();

        return product.Categories;
    }

    public async Task<Product> RemoverOrAddProductQuantityAsync(Guid productId, int quantity, MovementType type)
    {
        var product = await _context.Products
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == productId && p.DeletedAt == null);

        if (product == null)
            throw new KeyNotFoundException();

        if (type.Equals(MovementType.IN))
        {
            product.Quantity += quantity;
        }
        else
        {
            product.Quantity -= quantity;
            if (product.Quantity <= product.MinimumStock)
                _sendStockAlertHandler.handleAsync(product, quantity);
        }

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        var request = new CreateStockMovementRequest
        {
            ProductId = productId,
            Quantity = quantity,
            MovementType = type
        };
        await _stockMovementService.AddStockMovementToProduct(request);
        return product;
    }
}