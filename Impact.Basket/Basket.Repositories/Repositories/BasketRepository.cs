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
            using (var context = new BasketContext())
            {
                var basket = context.Baskets.Where(x => x.BasketId == basketId).Include(i => i.Items).FirstOrDefault();

                if (basket == null)
                    return null;

                b.BasketItemId = Guid.NewGuid();
                b.TotalPrice = b.Quantity * b.ProductUnitPrice;

                basket.TotalAmount += b.TotalPrice;

                basket.Items.Add(b);

                context.Update(basket);
                context.SaveChanges();

                return basket;
            }
        }

        public Models.Basket CreateBasket(Models.Basket b)
        {
            using (var context = new BasketContext())
            {
                b.BasketId = Guid.NewGuid();

                //Apply some business logic
                b.Items.ToList().ForEach(x =>
                {
                    x.BasketItemId = Guid.NewGuid();
                    x.TotalPrice = x.Quantity * x.ProductUnitPrice;
                });

                b.TotalAmount = b.Items.Sum(x => x.TotalPrice);

                context.Baskets.Add(b);
                context.SaveChanges();

                return b;
            }
        }

        public IList<Models.Basket> GetAllBaskets()
        {
            using (var context = new BasketContext())
            {
                var res = context.Baskets.Include(c => c.Items).ToList();

                return res;
            }
        }

        public Models.Basket? GetBasketById(Guid basketId)
        {
            using (var context = new BasketContext())
            {
                return context.Baskets.Where(x => x.BasketId == basketId).Include(i => i.Items).FirstOrDefault();
            }
        }

        public bool RemoveBasket(Guid basketId)
        {
            using (var context = new BasketContext())
            {
                var basket = context.Baskets.FirstOrDefault(x => x.BasketId == basketId);

                if (basket == null)
                    return false;

                context.Remove(basket);
                context.SaveChanges();

                return true;
            }
        }

        public Models.Basket? RemoveItemFromBasket(Guid basketId, BasketItem b)
        {
            using (var context = new BasketContext())
            {
                var basket = context.Baskets.Where(x => x.BasketId == basketId).Include(i => i.Items).FirstOrDefault();

                if (basket == null)
                    return null;

                basket.Items = basket.Items.Where(x => x.BasketItemId != b.BasketItemId).ToList();

                basket.TotalAmount -= b.TotalPrice;

                context.Update(basket);
                context.SaveChanges();

                return basket;
            }
        }

        public BasketItem? UpdateBasketItem(BasketItem b)
        {
            using (var context = new BasketContext())
            {
                var basketItem = context.BasketItems.FirstOrDefault(x => x.BasketItemId == b.BasketItemId);

                if (basketItem == null)
                    return null;

                basketItem.ProductSize = b.ProductSize;
                basketItem.ProductId = b.ProductId;
                basketItem.ProductName = b.ProductName;
                basketItem.Quantity = b.Quantity;
                basketItem.ProductUnitPrice = b.ProductUnitPrice;
                basketItem.TotalPrice = b.Quantity * b.ProductUnitPrice;

                context.BasketItems.Update(basketItem);
                context.SaveChanges();

                return basketItem;
            }
        }
    }
}
