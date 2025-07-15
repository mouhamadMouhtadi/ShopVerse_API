using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopVerse.API.Errors;
using ShopVerse.Core.Dtos.Auth;
using ShopVerse.Core.Entities.Identity;
using System.Security.Claims;

namespace ShopVerse.API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindByEmailWithAdsressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return null;

            var user = await userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null) return null;
            return user;
        }
        public static async Task<AddressDto> UpdateUserAdddresAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User,UpdateUserAddressDto updateAddressDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return null;

            var user = await userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null) return null;
            user.Address ??= new Address();
            user.Address.FName = updateAddressDto.FName;
            user.Address.LName = updateAddressDto.LName;
            user.Address.Street = updateAddressDto.Street;
            user.Address.City = updateAddressDto.City;
            user.Address.Country = updateAddressDto.Country;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded) return null;
            return new AddressDto()
            {
                FirstName = user.Address.FName,
                LastName = user.Address.LName,
                Street = user.Address.Street,
                City = user.Address.City,
                Country = user.Address.Country
            };

        }

    }
}
