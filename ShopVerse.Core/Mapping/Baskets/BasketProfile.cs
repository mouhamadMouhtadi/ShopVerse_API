using AutoMapper;
using ShopVerse.Core.Dtos.Basket;
using ShopVerse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Mapping.Baskets
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
        }
    }
}
