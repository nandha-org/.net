using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ThunderPay.Database.Sagas;

public class PaymentSagaDbContextFactory : IDesignTimeDbContextFactory<PaymentSagaDbContext>
{
    public PaymentSagaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentSagaDbContext>();
        optionsBuilder.UseNpgsql();

        return new PaymentSagaDbContext(optionsBuilder.Options);
    }
}