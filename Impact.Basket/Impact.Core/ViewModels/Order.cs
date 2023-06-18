using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Core.ViewModels
{
    public class Order
    {
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
