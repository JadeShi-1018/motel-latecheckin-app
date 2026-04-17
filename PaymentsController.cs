using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LateCheckInApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
    [HttpPost("create-preauth")]
    public async Task<IActionResult> CreatePreAuth([FromBody] CreatePreAuthRequest request)
    {
      var service = new PaymentIntentService();

      var options = new PaymentIntentCreateOptions
      {
        Amount = request.AmountInCents,
        Currency = "aud",
        CaptureMethod = "manual",
        PaymentMethodTypes = new List<string> { "card" }
      };

      var intent = await service.CreateAsync(options);

      return Ok(new
      {
        clientSecret = intent.ClientSecret,
        paymentIntentId = intent.Id
      });
    }
  }

  public class CreatePreAuthRequest
  {
    public long AmountInCents { get; set; }
  }
}

