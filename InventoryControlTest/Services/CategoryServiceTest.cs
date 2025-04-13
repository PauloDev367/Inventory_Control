using Moq;
using Microsoft.EntityFrameworkCore;
using InventoryControl.Data;
using InventoryControl.Models;
using InventoryControl.Requests;
using InventoryControl.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace InventoryControlTest.Services;

public class CategoryServiceTests
{
    private Mock<DbSet<Category>> CreateMockDbSet(List<Category> data)
    {
        var queryable = data.AsQueryable();

        var mockSet = new Mock<DbSet<Category>>();
        mockSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

        mockSet.Setup(d => d.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((c, _) => data.Add(c))
            .ReturnsAsync((Category c, CancellationToken _) =>
            {
                var mockEntry = new Mock<EntityEntry<Category>>();
                mockEntry.Setup(e => e.Entity).Returns(c);
                return mockEntry.Object;
            });

        mockSet.Setup(d => d.Update(It.IsAny<Category>()))
            .Callback<Category>(c =>
            {
                var index = data.FindIndex(x => x.Id == c.Id);
                if (index != -1) data[index] = c;
            });

        return mockSet;
    }


    private AppDbContext CreateMockContext(List<Category> data, out Mock<DbSet<Category>> mockSet)
    {
        mockSet = CreateMockDbSet(data);

        var mockContext = new Mock<AppDbContext>();
        mockContext.Setup(c => c.Categories).Returns(mockSet.Object);
        mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        return mockContext.Object;
    }

    [Fact]
    public async Task CreateCategoryAsync_ShouldAddCategory()
    {
        var data = new List<Category>();
        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var request = new CreateCategoryRequest { Name = "New Category" };

        var result = await service.CreateCategoryAsync(request);

        Assert.Single(data);
        Assert.Equal("New Category", result.Name);
    }

    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnPaginatedList()
    {
        var data = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Category 1" },
            new Category { Id = Guid.NewGuid(), Name = "Category 2" },
            new Category { Id = Guid.NewGuid(), Name = "Category 3", DeletedAt = DateTime.UtcNow },
        };

        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var result = await service.GetCategoriesAsync(1, 2);

        Assert.Equal(2, result.TotalItems);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetOneById_ShouldReturnCategory_WhenExists()
    {
        var category = new Category { Id = Guid.NewGuid(), Name = "Cat" };
        var data = new List<Category> { category };

        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var result = await service.GetOneById(category.Id);

        Assert.NotNull(result);
        Assert.Equal("Cat", result.Name);
    }

    [Fact]
    public async Task GetOneById_ShouldReturnNull_WhenNotFound()
    {
        var data = new List<Category>();
        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var result = await service.GetOneById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldUpdate_WhenExists()
    {
        var id = Guid.NewGuid();
        var category = new Category { Id = id, Name = "Old" };
        var data = new List<Category> { category };

        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var request = new UpdateCategoryRequest { Name = "Updated" };
        var result = await service.UpdateCategoryAsync(request, id);

        Assert.Equal("Updated", result.Name);
    }

    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrow_WhenNotFound()
    {
        var data = new List<Category>();
        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        var request = new UpdateCategoryRequest { Name = "Updated" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCategoryAsync(request, Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldMarkDeleted_WhenExists()
    {
        var id = Guid.NewGuid();
        var category = new Category { Id = id, Name = "ToDelete" };
        var data = new List<Category> { category };

        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        await service.DeleteCategoryAsync(id);

        Assert.NotNull(data.First().DeletedAt);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldThrow_WhenNotFound()
    {
        var data = new List<Category>();
        var context = CreateMockContext(data, out _);
        var service = new CategoryService(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteCategoryAsync(Guid.NewGuid()));
    }
}