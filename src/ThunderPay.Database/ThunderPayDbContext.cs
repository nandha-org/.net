using Microsoft.EntityFrameworkCore;
using ThunderPay.Database.Models;

namespace ThunderPay.Database;

public class ThunderPayDbContext(DbContextOptions<ThunderPayDbContext> options)
    : DbContext(options)
{
    public DbSet<MerchantDbm> Merchants { get; set; }

    public DbSet<OrganizationDbm> Organizations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(DatabaseConsts.ThunderPayDefaultSchema);

        MerchantDbm.Configure(modelBuilder.Entity<MerchantDbm>());
    }
}
