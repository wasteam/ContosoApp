using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AppStudio.Common.Cache
{
    public class AppCache
    {
        private static Dictionary<string, string> _memoryCache = new Dictionary<string, string>();

        public static async Task<CachedContent<T>> GetItemsAsync<T>(string key)
        {
            string json = null;
            if (_memoryCache.ContainsKey(key))
            {
                json = _memoryCache[key];
            }
            else
            {
                json = await UserStorage.ReadTextFromFile(key);
                _memoryCache[key] = json;
            }
            if (!String.IsNullOrEmpty(json))
            {
                try
                {
                    CachedContent<T> records = JsonConvert.DeserializeObject<CachedContent<T>>(json);
                    return records;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return null;
        }

        public static async Task AddItemsAsync<T>(string key, CachedContent<T> data, bool useStorageCache = true)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data);

                if (useStorageCache)
                {
                    await UserStorage.WriteText(key, json); 
                }

                if (_memoryCache.ContainsKey(key))
                {
                    _memoryCache[key] = json;
                }
                else
                {
                    _memoryCache.Add(key, json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
