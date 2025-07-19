using ShopVerse.Core.Dtos.Basket;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Entities.Order;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Services.Interfaces
{
    public interface IPaymentService
    {
       Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);
       Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag);
    }
}
