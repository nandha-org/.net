using MassTransit;
using Microsoft.Extensions.Logging;
using ThunderPay.Application.Sagas.EftSubmission.Messages;
using ThunderPay.Database.Sagas.EftSubmission;

namespace ThunderPay.Application.Sagas.EftSubmission.Activities;
public class SendNotificationActivity(ILogger<SendNotificationActivity> logger)
    : IStateMachineActivity<EftSubmissionSagaStateDbm, SendNotificationMsg>
{
    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<EftSubmissionSagaStateDbm, SendNotificationMsg> context, IBehavior<EftSubmissionSagaStateDbm, SendNotificationMsg> next)
    {
        logger.LogInformation("Sending notification for transaction: {TransactionId}", context.Message.TransactionId);

        await Task.Delay(1000);
        // Simulate sending notification logic here
        // In a real-world scenario, you would call an external notification service API
        await next.Execute(context);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<EftSubmissionSagaStateDbm, SendNotificationMsg, TException> context, IBehavior<EftSubmissionSagaStateDbm, SendNotificationMsg> next) where TException : Exception
    {
        logger.LogError(context.Exception, "Error sending notification for transaction: {TransactionId}", context.Message.TransactionId);
        return next.Faulted(context);
    }

    public void Probe(ProbeContext context)
    {
        context.CreateScope("send-notification");
    }
}
