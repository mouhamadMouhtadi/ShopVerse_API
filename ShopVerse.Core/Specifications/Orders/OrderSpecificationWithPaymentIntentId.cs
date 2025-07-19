using ShopVerse.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications.Orders
{
    public class OrderSpecificationWithPaymentIntentId : BaseSpecifications<Order,int>
    {
        public OrderSpecificationWithPaymentIntentId(string paymentIntentId) : base(x => x.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
