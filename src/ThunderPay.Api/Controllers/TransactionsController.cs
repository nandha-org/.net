using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ThunderPay.Application.Sagas.EftSubmission.Messages;

namespace ThunderPay.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionsController(IBus bus)
    : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await bus.Publish(new SubmitTransactionToProcessorMsg
        {
            TransactionId = Guid.NewGuid(),
            Amount = 100.00m,
        });

        // Implementation for getting organizations
        return this.Ok();
    }
}
