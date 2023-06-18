using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Core.ViewModels
{
    public class Basket
    {
        public Guid BasketId { get; set; }
        public string UserEmail { get; set; }

        public decimal TotalAmount { get; set; }
        public virtual IList<BasketItem> Items { get; set; }

    }
}
