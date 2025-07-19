using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using ShopVerse.Core;
using ShopVerse.Core.Dtos.Basket;
using ShopVerse.Core.Entities;
using ShopVerse.Core.Entities.Order;
using ShopVerse.Core.Services.Interfaces;
using ShopVerse.Core.Specifications.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = ShopVerse.Core.Entities.Product;

namespace ShopVerse.Services.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService,IUnitOfWork unitOfWork,IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;
            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }
            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                   var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(product.Price != item.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
                var Subtotal = basket.Items.Sum(x => x.Price * x.Quantity);

            var services = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(Subtotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>(){"card"},
                    Currency = "usd",
                };
                paymentIntent = await services.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }else
            {
                // Update 
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(Subtotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await services.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
             basket = await _basketService.UpdateBasketAsync(basket);
            if (basket is null) return null;    
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
           var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _unitOfWork.Repository<Order, int>().Update(order);
           await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
