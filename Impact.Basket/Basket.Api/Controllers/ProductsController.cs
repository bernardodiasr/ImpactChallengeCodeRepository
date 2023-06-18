using Basket.Contracts;
using Impact.Core.Contracts;
using Impact.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IIdentityProvider _identityProvider;
        private readonly ICodeChallengeConnector _codeChallengeConnector;


        public ProductsController(ILogger<BasketController> logger, IIdentityProvider identityProvider, ICodeChallengeConnector codeChallengeConnector)
        {
            _identityProvider = identityProvider;
            _logger = logger;
            _codeChallengeConnector = codeChallengeConnector;
        }

        [HttpGet, Route("GetBestRankedProducts")]
        public async Task<IActionResult> GetBestRankedProducts()
        {
            var productsDataSource = await _codeChallengeConnector.GetAllProducts().ConfigureAwait(false);

            var baseQuery = productsDataSource.OrderByDescending(x => x.Starts).Take(100).ToList();

            return Ok(baseQuery);
        }

        [HttpGet, Route("GetCheapestProducts")]
        public async Task<IActionResult> GetCheapestProducts()
        {
            var productsDataSource = await _codeChallengeConnector.GetAllProducts().ConfigureAwait(false);

            var baseQuery = productsDataSource.OrderBy(x => x.Price).Take(10).ToList();

            return Ok(baseQuery);
        }

        [HttpGet, Route("GetPagedProducts")]
        public async Task<IActionResult> GetPagedProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            if (pageSize > 1000)
                return BadRequest("The page size cannot exceed 1000.");

            if (page <= 0)
                return BadRequest("The page cannot be 0 or less.");

            var productsDataSource = await _codeChallengeConnector.GetAllProducts().ConfigureAwait(false);

            var baseQuery = productsDataSource.OrderBy(x => x.Price).ToList();

            var totalItems = productsDataSource.Count();

            var totalPages = totalItems % pageSize == 0 ? totalItems / pageSize : (totalItems / pageSize) + 1;

            if (page > totalPages)
                return BadRequest($"There are only {totalPages} pages.");

            var skipCount = page == 1 ? 0 : (page - 1) * pageSize;

            var resultItems = baseQuery.Skip(skipCount).Take(pageSize).ToList();

            var pagedResult = new PagedResult<Product>()
            {
                Results = resultItems,
                Page = new Page()
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                }
            };


            return Ok(pagedResult);
        }

    }
}
