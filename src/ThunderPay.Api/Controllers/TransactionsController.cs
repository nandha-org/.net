using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ThunderPay.Application.Sagas.EftSubmission.Messages;

namespace ThunderPay.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(IBus bus)
    : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]TransactionRequest request)
    {
        await bus.Publish(new SubmitTransactionToProcessorMsg
        {
            TransactionId = request.TransactionId,
            ToFail = request.ToFail,
            Amount = 100.00m,
        });

        //await bus.Publish(new PostTransactionToWalletMsg
        //{
        //    TransactionId = request.TransactionId,
        //});

        // Implementation for getting organizations
        return this.Ok();
    }
}
