namespace ThunderPay.Application.Sagas.EftSubmission.Messages;
public class PostTransactionToWalletMsg
{
    public required Guid TransactionId { get; set; }
}
