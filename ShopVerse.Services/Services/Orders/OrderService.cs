using Microsoft.Identity.Client;
using ShopVerse.Core;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Entities.Order;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Services.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork , IBasketService basketService,IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            if(basket is null) return null;
            var orderItems = new List<OrderItem>();
            if (basket.Items.Count()> 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product,int>().GetAsync(item.Id);
                    var productOrderItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderItem,product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod,int>().GetAsync(deliveryMethodId);


            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var existingOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                _unitOfWork.Repository<Order , int>().Delete(existingOrder);
            }
           var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var order = new Order(buyerEmail, shippingAddress,deliveryMethod,orderItems, subTotal,basketDto.PaymentIntentId);

           await _unitOfWork.Repository<Order, int>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null; // Order creation failed
            return order;
        }  

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {

            var spec = new OrderSpecifications(buyerEmail,orderId);
          var order = await  _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null; 
            return order;
        }

        public Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
           var orders =  _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if (orders is null) return null;
            return orders;
        }
    }
}
