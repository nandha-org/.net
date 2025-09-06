namespace ThunderPay.Application.Sagas.EftSubmission.Messages;
public class SendNotificationMsg
{
    public required Guid TransactionId { get; set; }
}
