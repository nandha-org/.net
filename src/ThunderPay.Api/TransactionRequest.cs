namespace ThunderPay.Api;
public class TransactionRequest
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    public bool ToFail { get; set; } = false;
}
