﻿using ShopVerse.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Services.Interfaces
{
    public interface IUserService
    {
       Task<UserDto> LoginAsync(LoginDto loginDto);
         Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> CheckEmailExistAsync(string email);
    }
}
