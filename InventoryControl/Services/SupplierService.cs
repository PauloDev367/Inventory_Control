using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class SupplierService
{
     private readonly AppDbContext _context;

    public SupplierService(AppDbContext context) => _context = context;
    
    public async Task<PaginatedResultResponseRequest<Supplier>> GetCategoriesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Suppliers.AsQueryable();
        var totalItems = await query.CountAsync();

        var categories = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<Supplier>
        {
            Items = categories,
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
    }
}