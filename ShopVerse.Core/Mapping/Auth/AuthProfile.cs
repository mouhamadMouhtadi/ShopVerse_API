using AutoMapper;
using ShopVerse.Core.Dtos.Auth;
using ShopVerse.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Mapping.Auth
{
    public   class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
