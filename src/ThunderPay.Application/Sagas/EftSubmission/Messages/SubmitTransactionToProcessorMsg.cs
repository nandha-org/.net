namespace ThunderPay.Application.Sagas.EftSubmission.Messages;
public class SubmitTransactionToProcessorMsg
{
    public required Guid TransactionId { get; set; }

    public required decimal Amount { get; set; }
}
