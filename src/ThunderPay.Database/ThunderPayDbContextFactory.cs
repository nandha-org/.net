using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ThunderPay.Database;

public class ThunderPayDbContextFactory : IDesignTimeDbContextFactory<ThunderPayDbContext>
{
    public ThunderPayDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ThunderPayDbContext>();
        optionsBuilder.UseNpgsql();

        return new ThunderPayDbContext(optionsBuilder.Options);
    }
}