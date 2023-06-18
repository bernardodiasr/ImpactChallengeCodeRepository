using Autofac;
using Basket.Contracts;
using Basket.Repositories.Repositories;

namespace Basket.Tests
{
    public class DbFixture
    {
        public Autofac.IContainer Container { get; private set; }

        public DbFixture()
        {

            var builder = new ContainerBuilder();

            builder.RegisterType<BasketRepository>().As<IBasketRepository>();

            Container = builder.Build();
        }
    }

    public class BasketRepositoryTests : IClassFixture<DbFixture>
    {
        private readonly IBasketRepository _repository;
        public BasketRepositoryTests(DbFixture fixture)
        {
            _repository = fixture.Container.Resolve<IBasketRepository>();
        }

        [Fact]
        public async Task GetBaskets()
        {
            var results = _repository.GetAllBaskets();

            Assert.NotEmpty(results);

        }

        [Fact]
        public async Task CreateBasket()
        {
            var newBasket = new Models.Basket()
            {
                BasketId = Guid.NewGuid(),
                TotalAmount = 0,
                UserEmail = "teste12345@gmail.com",
                Items = new List<Models.BasketItem>()
                {
                    new Models.BasketItem() {
                    BasketItemId = Guid.NewGuid(),
                    ProductId = 1,
                    ProductName = "Test",
                    ProductSize = "ProductSize",
                    ProductUnitPrice = 2,
                    Quantity = 2,
                    TotalPrice = 0
                    },
                    new Models.BasketItem() {
                                BasketItemId = Guid.NewGuid(),
                    ProductId = 1,
                    ProductName = "Test",
                    ProductSize = "ProductSize",
                    ProductUnitPrice = 5,
                    Quantity = 4,
                    TotalPrice = 0
                    }
                }
            };

            var newObj = _repository.CreateBasket(newBasket);

            Assert.NotNull(newObj);

            Assert.Equal(24, newObj.TotalAmount);
        }
    }
}