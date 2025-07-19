using ShopVerse.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail,string basketId, int deliveryMethodId, Address shippingAddress);
        Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail,int orderId);
        Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail);
    }
}
