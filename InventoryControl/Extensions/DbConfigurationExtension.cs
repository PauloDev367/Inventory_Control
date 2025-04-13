using InventoryControl.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Extensions;

public static class DbConfigurationExtension
{
    public static void ConfigureDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        string connString = configuration.GetConnectionString("SqlServer");
        service.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connString);
        });
    }

}