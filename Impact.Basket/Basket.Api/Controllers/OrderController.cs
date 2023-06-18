using Impact.Core.Contracts;
using Impact.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICodeChallengeConnector _codeChallengeConnector;


        public OrderController(ILogger<BasketController> logger, IIdentityProvider identityProvider, ICodeChallengeConnector codeChallengeConnector)
        {
            _identityProvider = identityProvider;
            _logger = logger;
            _codeChallengeConnector = codeChallengeConnector;
        }

        [HttpPost, Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] Order o)
        {
            var orderCreated = await _codeChallengeConnector.CreateOrder(o).ConfigureAwait(false);

            if (orderCreated == null)
                return BadRequest();

            return Ok(orderCreated);
        }

        [HttpGet, Route("GetOrder/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            var order = await _codeChallengeConnector.GetOrder(orderId).ConfigureAwait(false);

            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
