using Basket.Contracts;
using Basket.Models;
using Basket.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Repositories.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        public BasketRepository()
        {
            //Set some dummy data
            using (var context = new BasketContext())
            {
                var basketPreOrder = new Models.Basket()
                {
                    BasketId = Guid.Parse("fbf7f4ef-76a6-451b-a48d-0c478fe9c0a0"), 
                    TotalAmount = 3,
                    UserEmail = "bernardo.diasr@gmail.com",
                    Items = new List<BasketItem>() {
                        new BasketItem()
                        {
                            ProductId = 1,
                            ProductName = "Test_1",
                            ProductSize = "1",
                            ProductUnitPrice = 1,
                            Quantity = 1,
                            TotalPrice = 1
                        },
                        new BasketItem()
                        {
                            ProductId = 2,
                            ProductName = "Test_2",
                            ProductSize = "2",
                            ProductUnitPrice = 1,
                            Quantity = 2,
                            TotalPrice = 2
                        }
                    }

                };

                if (!context.Baskets.Any(x => x.BasketId == basketPreOrder.BasketId))
                {
                    context.Baskets.Add(basketPreOrder);
                    context.SaveChanges();
                }
            }
        }


        public Models.Basket AddItemToBasket(Guid basketId, BasketItem b)
        {
            throw new NotImplementedException();
        }

        public Models.Basket CreateBasket(Models.Basket b)
        {
            throw new NotImplementedException();
        }

        public IList<Models.Basket> GetAllBaskets()
        {
            throw new NotImplementedException();
        }

        public Models.Basket? GetBasketById(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveBasket(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public Models.Basket? RemoveItemFromBasket(Guid basketId, BasketItem b)
        {
            throw new NotImplementedException();
        }

        public BasketItem? UpdateBasketItem(BasketItem b)
        {
            throw new NotImplementedException();
        }
    }
}
