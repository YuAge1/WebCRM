using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCRM.Application.Models.Carts;

namespace WebCRM.Application.Models.Orders
{
    public class CreateOrderDto
    {
        public string? Name { get; set; }
        public long OrderNumber { get; set; }
        public long CustomerId { get; set; }
        public CartDto? Cart { get; set; }
    }
}
