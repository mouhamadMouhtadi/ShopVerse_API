using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopVerse.Core.Services.Interfaces
{
    public interface ICasheService
    {
        Task SetCasheKeyAsync(string key, object response, TimeSpan expireTime);
        Task<string> GetCasheKeyAsync(string key);
    }
}
