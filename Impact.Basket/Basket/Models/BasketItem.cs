using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Models
{
    public class BasketItem
    {
        [Key]
        public Guid BasketItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public string ProductSize { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
