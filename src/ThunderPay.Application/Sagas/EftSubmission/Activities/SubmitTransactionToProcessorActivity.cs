using System.Diagnostics;
using System.Text.Json;
using MassTransit;
using Microsoft.Extensions.Logging;
using ThunderPay.Application.Sagas.EftSubmission.Messages;
using ThunderPay.Database.Sagas.EftSubmission;

namespace ThunderPay.Application.Sagas.EftSubmission.Activities;
public class SubmitTransactionToProcessorActivity(ILogger<SubmitTransactionToProcessorActivity> logger)
    : IStateMachineActivity<EftSubmissionSagaStateDbm, SubmitTransactionToProcessorMsg>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EftSubmissionSagaStateDbm, SubmitTransactionToProcessorMsg> context, IBehavior<EftSubmissionSagaStateDbm, SubmitTransactionToProcessorMsg> next)
    {
        logger.LogInformation("Submitting transaction to processor: {TransactionId}, Amount: {Amount}", context.Message.TransactionId, context.Message.Amount);
        Debug.WriteLine($"Submitting transaction to processor: {JsonSerializer.Serialize(context.Message)}");

        // delay to simulate processing
        await Task.Delay(1000);

        // Simulate submission logic here
        // In a real-world scenario, you would call an external payment processor API
        await next.Execute(context);
        await context.Publish(new PostTransactionToWalletMsg
        {
            TransactionId = context.Message.TransactionId,
        });
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EftSubmissionSagaStateDbm, SubmitTransactionToProcessorMsg, TException> context, IBehavior<EftSubmissionSagaStateDbm, SubmitTransactionToProcessorMsg> next)
        where TException : Exception
    {
        logger.LogError(context.Exception, "Error submitting transaction to processor: {TransactionId}", context.Message.TransactionId);
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("submit-transaction-to-processor");
    }
}
