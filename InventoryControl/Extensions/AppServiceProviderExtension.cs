using InventoryControl.Services;

namespace InventoryControl.Extensions;

public static class AppServiceProviderExtension
{
    public static void LoadDependencies(this IServiceCollection service)
    {
        service.AddTransient<IdentityService, IdentityService>();
        service.AddTransient<ProductService, ProductService>();
    }

}
