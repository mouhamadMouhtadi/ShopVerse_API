using Microsoft.AspNetCore.Identity;
using ShopVerse.Core.Entities.Identity;
using ShopVerse.Repository.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public  async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "mhamadmouhtadi@gmail.com",
                    DisplayName = "Mhamad Mouhtadi ",
                    UserName = "Mhamad.Mouhtadi",
                    PhoneNumber = "000111222",
                    Address = new Address()
                    {
                        FName = "Mhamad",
                        LName = "Mouhtadi",
                        Street = "Main Road",
                        City = "Halba",
                        Country = "Lebanon"
                    }
                };
               await _userManager.CreateAsync(user,"P@ssw0rd");
            }
        }
    };
}