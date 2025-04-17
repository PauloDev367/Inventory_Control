using InventoryControl.Services;

namespace InventoryControl.Extensions;

public static class AppServiceProviderExtension
{
    public static void LoadDependencies(this IServiceCollection service)
    {
        service.AddTransient<IdentityService, IdentityService>();
        service.AddTransient<ProductService, ProductService>();
        service.AddTransient<CategoryService, CategoryService>();
        service.AddTransient<SupplierService, SupplierService>();
        service.AddTransient<StockMovementService, StockMovementService>();
        service.AddTransient<ProductPriceService, ProductPriceService>();
        service.AddTransient<SaleService, SaleService>();
    }
}