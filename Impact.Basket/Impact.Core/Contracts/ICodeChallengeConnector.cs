using Impact.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Core.Contracts
{
    public interface ICodeChallengeConnector
    {

        Task<IList<Product>> GetAllProducts();

        Task<Order> CreateOrder(Order order);
        Task<Order> GetOrder(string orderId);
    }
}
