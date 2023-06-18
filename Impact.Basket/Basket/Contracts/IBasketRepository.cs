using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Contracts
{
    public interface IBasketRepository
    {
        IList<Models.Basket> GetAllBaskets();
        Models.Basket? GetBasketById(Guid basketId);
        Models.Basket CreateBasket(Models.Basket b);
        bool RemoveBasket(Guid basketId);
        Models.BasketItem? UpdateBasketItem(Models.BasketItem b);
        Models.Basket AddItemToBasket(Guid basketId, Models.BasketItem b);
        Models.Basket? RemoveItemFromBasket(Guid basketId, Models.BasketItem b);
    }
}
