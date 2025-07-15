using ShopVerse.Core.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopVerse.Services.Services.Cashes
{
    public class CasheService : ICasheService
    {
        private readonly IDatabase _database;
        public CasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<string> GetCasheKeyAsync(string key)
        {
          var casheResponse =  await _database.StringGetAsync(key);
            if (casheResponse.IsNullOrEmpty)
                return null;
            return casheResponse.ToString();
        }

        public async Task SetCasheKeyAsync(string key, object response, TimeSpan expireTime)
        {
            if (response is null) return;
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            await _database.StringSetAsync(key,JsonSerializer.Serialize(response, options), expireTime);
        }
    }
}
