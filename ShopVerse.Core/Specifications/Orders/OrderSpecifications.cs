using ShopVerse.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order, int>
    {
        public OrderSpecifications(string buyerEmail, int OrderId) : base(O => O.BuyerEmail == buyerEmail && O.Id == OrderId)
        {
            ApplyOrderSpec();
        }
        public OrderSpecifications(string buyerEmail) : base(O => O.BuyerEmail == buyerEmail)
        {
            ApplyOrderSpec();
        }

        private void ApplyOrderSpec ()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
