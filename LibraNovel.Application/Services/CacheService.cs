﻿using LibraNovel.Application.Interfaces;
using LibraNovel.Application.ViewModels.Novel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraNovel.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public CacheService(IDistributedCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);
            _database = _redis.GetDatabase();
        }

        //Get data from cache
        public async Task<string?> GetDataFromCache<T>(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        //Remove cache by key fragment
        public async Task RemoveCacheKeysContaining(string keyFragment)
        {
            var cacheKeys = GetAllCacheKeys();

            foreach (var cacheKey in cacheKeys)
            {
                if (cacheKey.StartsWith(keyFragment))
                {
                    await _cache.RemoveAsync(cacheKey);
                }
            }
        }

        //Store data to cache
        public async Task<T> StoreDataToCache<T>(string key, T data)
        {
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return data;
        }

        //Get all cache key
        private List<string> GetAllCacheKeys()
        {
            var keys = new List<string>();

            // Get all keys using SCAN command (use pattern "*" to match all keys)
            var redisKeys = _redis.GetServer(_redis.GetEndPoints()[0]).Keys();
            foreach (var key in redisKeys)
            {
                keys.Add(key.ToString());
            }

            return keys;
        }
    }
}
