using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCRM.Application.Models.Carts;

namespace WebCRM.Application.Abstractions
{
    public interface ICartsService
    {
        Task<CartDto> Create(CartDto cart);
    }
}
