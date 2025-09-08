using MassTransit;
using ThunderPay.Application.Sagas.EftSubmission.Activities;
using ThunderPay.Application.Sagas.EftSubmission.Messages;
using ThunderPay.Database.Sagas.EftSubmission;

namespace ThunderPay.Application.Sagas.EftSubmission;
internal class EftSubmissionStateMachine : MassTransitStateMachine<EftSubmissionSagaStateDbm>
{
    public EftSubmissionStateMachine()
    {
        this.InstanceState(x => x.CurrentState);
        this.DefineStates();
        this.InitEvents();
        this.DefineBehaviors();
    }

    public State SubmittedToProcessor { get; private set; } = default!;

    public State PostedToWallet { get; private set; } = default!;

    public State NotificationSent { get; private set; } = default!;

    public State ProcessorFailed { get; private set; } = default!;

    public State WalletFailed { get; private set; } = default!;

    public State NotificationFailed { get; private set; } = default!;

    public Event<SubmitTransactionToProcessorMsg> SubmitTransactionToProcessor { get; private set; } = default!;

    public Event<PostTransactionToWalletMsg> PostTransactionToWallet { get; private set; } = default!;

    public Event<SendNotificationMsg> SendNotification { get; private set; } = default!;

    private void DefineStates()
    {
        this.State(() => this.SubmittedToProcessor);
        this.State(() => this.PostedToWallet);
        this.State(() => this.NotificationSent);
        this.State(() => this.ProcessorFailed);
        this.State(() => this.WalletFailed);
        this.State(() => this.NotificationFailed);
    }

    private void InitEvents()
    {
        this.Event(() => this.SubmitTransactionToProcessor, x =>
        {
            x.CorrelateById(ctx => ctx.Message.TransactionId);
            x.InsertOnInitial = true;
            x.SetSagaFactory(context => new ()
            {
                CorrelationId = context.Message.TransactionId,
                CurrentState = this.Initial.Name,
            });
        });

        this.Event(() => this.PostTransactionToWallet, x => x.CorrelateById(ctx => ctx.Message.TransactionId));
        this.Event(() => this.SendNotification, x => x.CorrelateById(ctx => ctx.Message.TransactionId));
    }

    private void DefineBehaviors()
    {
        this.Initially(
            this.When(this.SubmitTransactionToProcessor)
                .Activity(x => x.OfType<SubmitTransactionToProcessorActivity>())
                .TransitionTo(this.SubmittedToProcessor)
                .Catch<Exception>(ex => ex.TransitionTo(this.ProcessorFailed)));

        this.During(
            this.SubmittedToProcessor,
            this.When(this.PostTransactionToWallet)
                .Activity(x => x.OfType<PostTransactionToWalletActivity>())
                .TransitionTo(this.PostedToWallet)
                .Catch<Exception>(ex => ex.TransitionTo(this.WalletFailed)));

        this.During(
            this.PostedToWallet,
            this.When(this.SendNotification)
                .Activity(x => x.OfType<SendNotificationActivity>())
                .TransitionTo(this.NotificationSent)
                .Finalize()
                .Catch<Exception>(ex => ex.TransitionTo(this.NotificationFailed)));

        // Allow manual retry from failed states
        this.During(
            this.ProcessorFailed,
            this.When(this.SubmitTransactionToProcessor)
                .Activity(x => x.OfType<SubmitTransactionToProcessorActivity>())
                .TransitionTo(this.SubmittedToProcessor)
                .Catch<Exception>(ex => ex.TransitionTo(this.ProcessorFailed)));

        this.During(
            this.WalletFailed,
            this.When(this.PostTransactionToWallet)
                .Activity(x => x.OfType<PostTransactionToWalletActivity>())
                .TransitionTo(this.PostedToWallet)
                .Catch<Exception>(ex => ex.TransitionTo(this.WalletFailed)));

        this.During(
            this.NotificationFailed,
            this.When(this.SendNotification)
                .Activity(x => x.OfType<SendNotificationActivity>())
                .TransitionTo(this.NotificationSent)
                .Finalize()
                .Catch<Exception>(ex => ex.TransitionTo(this.NotificationFailed)));
    }
}
