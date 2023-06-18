using Basket.Contracts;
using Impact.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IIdentityProvider _identityProvider;
        private readonly IBasketRepository _basketRepository;


        public BasketController(ILogger<BasketController> logger, IIdentityProvider identityProvider, IBasketRepository basketRepository)
        {
            _identityProvider = identityProvider;
            _logger = logger;
            _basketRepository = basketRepository;
        }

        //Mappers
        private Impact.Core.ViewModels.Basket MapToViewModel(Models.Basket b)
        {
            return new Impact.Core.ViewModels.Basket()
            {
                BasketId = b.BasketId,
                TotalAmount = b.TotalAmount,
                UserEmail = b.UserEmail,
                Items = b.Items.Any() ? b.Items.Select(p => new Impact.Core.ViewModels.BasketItem()
                {
                    BasketItemId = p.BasketItemId,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductSize = p.ProductSize,
                    ProductUnitPrice = p.ProductUnitPrice,
                    Quantity = p.Quantity,
                    TotalPrice = p.TotalPrice
                }).ToList() : new List<Impact.Core.ViewModels.BasketItem>() { }
            };
        }

        private Models.Basket MapToModel(Impact.Core.ViewModels.Basket b)
        {
            return new Models.Basket()
            {
                BasketId = b.BasketId,
                TotalAmount = b.TotalAmount,
                UserEmail = b.UserEmail,
                Items = b.Items.Any() ? b.Items.Select(p => new Models.BasketItem()
                {
                    BasketItemId = p.BasketItemId,
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    ProductSize = p.ProductSize,
                    ProductUnitPrice = p.ProductUnitPrice,
                    Quantity = p.Quantity,
                    TotalPrice = p.TotalPrice
                }).ToList() : new List<Models.BasketItem>() { }
            };
        }

        private Impact.Core.ViewModels.BasketItem MapItemToViewModel(Models.BasketItem i)
        {
            return new Impact.Core.ViewModels.BasketItem()
            {
                BasketItemId = i.BasketItemId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductSize = i.ProductSize,
                ProductUnitPrice = i.ProductUnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            };
        }

        private Models.BasketItem MapItemToModel(Impact.Core.ViewModels.BasketItem i)
        {
            return new Models.BasketItem()
            {
                BasketItemId = i.BasketItemId,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                ProductSize = i.ProductSize,
                ProductUnitPrice = i.ProductUnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            };
        }

        [HttpGet, Route("GetBasket/{basketId}")]
        public IActionResult GetBasket(Guid basketId)
        {
            var basketModel = _basketRepository.GetBasketById(basketId);

            if (basketModel == null)
                return NotFound();

            var basketViewModel = MapToViewModel(basketModel);

            return Ok(basketViewModel);
        }

        [HttpPost, Route("CreateBasket")]
        public IActionResult CreateBasket([FromBody] Impact.Core.ViewModels.Basket b)
        {
            var model = MapToModel(b);

            model.UserEmail = _identityProvider.Email;

            var resModel = _basketRepository.CreateBasket(model);

            var resViewModel = MapToViewModel(resModel);

            return Ok(resViewModel);
        }

        [HttpDelete, Route("RemoveBasket/{basketId}")]
        public IActionResult RemoveBasket(Guid basketId)
        {
            var wasSuccess = _basketRepository.RemoveBasket(basketId);

            return Ok(wasSuccess);
        }

        [HttpPut, Route("UpdateItem")]
        public IActionResult UpdateItem([FromBody] Impact.Core.ViewModels.BasketItem b)
        {
            var model = MapItemToModel(b);

            var resModel = _basketRepository.UpdateBasketItem(model);

            if (resModel == null)
                return NotFound();

            var resViewModel = MapItemToViewModel(resModel);

            return Ok(resViewModel);
        }

        [HttpPost, Route("AddProductToBasket")]
        public IActionResult AddProductToBasket(Guid basketId, [FromBody] Impact.Core.ViewModels.BasketItem basketItem)
        {
            var modelItem = MapItemToModel(basketItem);

            var basketModel = _basketRepository.AddItemToBasket(basketId, modelItem);

            var basketViewModel = MapToViewModel(basketModel);

            return Ok(basketViewModel);
        }

        [HttpPost, Route("RemoveProductFromBasket")]
        public IActionResult RemoveProductFromBasket(Guid basketId, [FromBody] Impact.Core.ViewModels.BasketItem basketItem)
        {
            var modelItem = MapItemToModel(basketItem);

            var basketModel = _basketRepository.RemoveItemFromBasket(basketId, modelItem);

            var basketViewModel = MapToViewModel(basketModel);

            return Ok(basketViewModel);
        }

        [HttpGet, Route("GetAllBaskets")]
        public IActionResult GetAllBaskets()
        {
            var baskets = _basketRepository.GetAllBaskets().ToList();

            var basketViewModels = baskets.Select(x => MapToViewModel(x)).ToList();

            return Ok(basketViewModels);
        }
    }
}
