using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace In_Memory_Caching_ASP_DOT_NET_Core.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class InfoController : ControllerBase {
        private readonly IMemoryCache _memoryCache;
        public InfoController (IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public string GetInfo () {
            var key = "info1";
            if (!_memoryCache.TryGetValue (key, out string info)) {
                info = getInformation ();
                var expirationOptions = new MemoryCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromSeconds (3),
                    Priority = CacheItemPriority.Normal,
                    AbsoluteExpiration = DateTime.Now.AddSeconds(10)
                };
                _memoryCache.Set (key, info, expirationOptions);
            }
            return info;
        }
        private string getInformation () {
            return $"this is a sample text. generated at : {DateTime.Now.ToLongTimeString()}";
        }
    }
}