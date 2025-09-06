using MassTransit;
using Microsoft.Extensions.Logging;
using ThunderPay.Application.Sagas.EftSubmission.Messages;
using ThunderPay.Database.Sagas.EftSubmission;

namespace ThunderPay.Application.Sagas.EftSubmission.Activities;
public class PostTransactionToWalletActivity(ILogger<PostTransactionToWalletActivity> logger)
    : IStateMachineActivity<EftSubmissionSagaStateDbm, PostTransactionToWalletMsg>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EftSubmissionSagaStateDbm, PostTransactionToWalletMsg> context, IBehavior<EftSubmissionSagaStateDbm, PostTransactionToWalletMsg> next)
    {
        logger.LogInformation("Posting transaction to wallet: {TransactionId}", context.Message.TransactionId);
        await Task.Delay(1000);

        // Simulate posting logic here
        // In a real-world scenario, you would call an external wallet service API
        await next.Execute(context);
        await context.Publish(new SendNotificationMsg
        {
            TransactionId = context.Message.TransactionId,
        });
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EftSubmissionSagaStateDbm, PostTransactionToWalletMsg, TException> context, IBehavior<EftSubmissionSagaStateDbm, PostTransactionToWalletMsg> next) where TException : Exception
    {
        logger.LogError(context.Exception, "Error posting transaction to wallet: {TransactionId}", context.Message.TransactionId);
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("post-transaction-to-wallet");
    }
}
