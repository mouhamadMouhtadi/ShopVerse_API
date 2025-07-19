using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopVerse.API.Errors;
using ShopVerse.Core.Services.Interfaces;
using Stripe;
using System.Threading.Tasks;

namespace ShopVerse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntentId(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest, "Basket ID cannot be null."));
            var basket = await  _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "whsec_d8d737c10fe2973b2ee9d988fa5e2530a8a5d9d8b0261bf8c29757647a7d6a6d";

        [HttpPost("webhook")] // https://localhost:7257/api/payments/webhook
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
              var paymentIntent =   stripeEvent.Data.Object as PaymentIntent;

                // Handle the event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    // Update Db 
                   await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == "payment_intent.payment_succeeded")
                {
                    // update Db
                    await _paymentService.UpdatePaymentIntentForSucceedOrFailed(paymentIntent.Id, true);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
