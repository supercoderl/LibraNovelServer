using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.Interfaces
{
    public interface ICacheService
    {
        Task RemoveCacheKeysContaining(string keyFragment);
        Task<T> StoreDataToCache<T>(string key, T data);
        Task<string?> GetDataFromCache<T>(string key);
    }
}
