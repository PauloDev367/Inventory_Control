using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class SupplierService
{
     private readonly AppDbContext _context;

    public SupplierService(AppDbContext context) => _context = context;
    
    public async Task<PaginatedResultResponseRequest<Supplier>> GetSupplierAsync(int pageNumber, int pageSize)
    {
        var query = _context.Suppliers.AsQueryable();
        var totalItems = await query.CountAsync();

        var supplier = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<Supplier>
        {
            Items = supplier,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
    
    public async Task<Supplier> CreateSupplierAsync(CreateSupplierRequest request)
    {
        var supplier = new Supplier()
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
        };

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier> UpdateSupplierAsync(UpdateSupplierRequest request, Guid supplierId)
    {
        var supplier = await _context.Suppliers
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(supplierId));

        if (supplier == null)
            throw new KeyNotFoundException();

        supplier.Name = request.Name;
        supplier.Email = request.Email;
        supplier.PhoneNumber = request.PhoneNumber;
        
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier?> GetOneById(Guid supplierId)
    {
        return await _context.Suppliers
            .Where(p => p.DeletedAt == null)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(supplierId));
    }

    public async Task DeleteSupplierAsync(Guid supplierId)
    {
        var supplier = await _context.Suppliers
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(supplierId));

        if (supplier == null)
            throw new KeyNotFoundException();

        supplier.DeletedAt = DateTime.Now;
        _context.Suppliers.Update(supplier);
        await _context.SaveChangesAsync();
        
        var products = await _context.Products
            .Where(p => p.SupplierId.Equals(supplierId))
            .ToListAsync();
        foreach (var product in products)
        {
            product.SupplierId = null;
            _context.Products.Update(product);
        }
        await _context.SaveChangesAsync();
    }

    public async Task AddSupplierToProductAsync(Guid supplierId, Guid productId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p=>p.Id.Equals(productId));
        if (product == null)
            throw new KeyNotFoundException();
        
        product.SupplierId = supplierId;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveSupplierFromProductAsync(Guid supplierId, Guid productId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p=>p.Id.Equals(productId));
        if (product == null)
            throw new KeyNotFoundException();
        product.SupplierId = null;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<PaginatedResultResponseRequest<Product>> GetSupplierProductsAsync(int pageNumber, int pageSize, Guid supplierId)
    {
        var query = _context.Products.AsQueryable();
        var totalItems = await query.CountAsync();

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Where(p=>p.SupplierId.Equals(supplierId))
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