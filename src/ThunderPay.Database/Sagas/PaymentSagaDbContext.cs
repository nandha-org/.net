using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using ThunderPay.Database.Sagas.EftSubmission;

namespace ThunderPay.Database.Sagas;
public class PaymentSagaDbContext : SagaDbContext
{
    public PaymentSagaDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new EftSubmissionSagaStateMap(); }
    }
}
