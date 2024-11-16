using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Threading.Tasks;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Interfaces;
using Stripe.Checkout;

[Route("api/[controller]")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;
    private readonly IOrderService _orderService;
    private readonly IConfiguration _configuration;

    public StripeController(IStripeService stripeService, IOrderService orderService, IConfiguration configuration)
    {
        _stripeService = stripeService;
        _orderService = orderService;
        _configuration = configuration;
    }
    [HttpPost("create-checkout-session")]
    public async Task<ActionResult> CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
    {
        var domain = _configuration["App:Domain"];
        var successUrl = domain + "/payment-success?session_id={CHECKOUT_SESSION_ID}";
        var cancelUrl = domain + "/cancel";
    
        var session = await _stripeService.CreateCheckoutSessionAsync(
            request.CartItems, 
            successUrl, 
            cancelUrl,
            request.CouponCode
        );
    
        return Ok(new { sessionId = session.Id });
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {      
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _configuration["Stripe:WebhookSecret"]
            );

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                await _orderService.CreateOrderFromSession(session);
            }

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
    [HttpPost("refund")]
    public async Task<IActionResult> Refund(int id)
    {
        try
        {
            var refund = await _stripeService.Refund(id);
            return Ok(refund);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}