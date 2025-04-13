using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Services;

public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context) => _context = context;
    
    public async Task<PaginatedResultResponseRequest<Category>> GetCategoriesAsync(int pageNumber, int pageSize)
    {
        var query = _context.Categories.AsQueryable();
        var totalItems = await query.CountAsync();

        var categories = await query
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking()
            .Where(p => p.DeletedAt == null)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResultResponseRequest<Category>
        {
            Items = categories,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
    
    public async Task<Category> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category()
        {
            Name = request.Name
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(UpdateCategoryRequest request, Guid categoryId)
    {
        var category = await _context.Categories
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(categoryId));

        if (category == null)
            throw new KeyNotFoundException();

        category.Name = request.Name;
        
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> GetOneById(Guid categoryId)
    {
        return await _context.Categories
            .Where(p => p.DeletedAt == null)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id.Equals(categoryId));
    }

    public async Task DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _context.Categories
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id.Equals(categoryId));

        if (category == null)
            throw new KeyNotFoundException();

        category.DeletedAt = DateTime.Now;
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
}