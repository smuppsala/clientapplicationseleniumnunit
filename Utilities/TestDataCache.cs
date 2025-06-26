using System.Collections.Concurrent;

namespace ClientApplicationTestProject.Utilities
{
    /// <summary>
    /// Static cache utility for storing test data that needs to be accessed across test classes
    /// </summary>
    public static class TestDataCache
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Adds or updates a value in the cache
        /// </summary>
        public static void Set<T>(string key, T value)
        {
            _cache.AddOrUpdate(key, value, (_, _) => value);
        }

        /// <summary>
        /// Gets a value from the cache
        /// </summary>
        public static T Get<T>(string key)
        {
            if (_cache.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }

            return default;
        }

        /// <summary>
        /// Removes a value from the cache
        /// </summary>
        public static bool Remove(string key)
        {
            return _cache.TryRemove(key, out _);
        }

        /// <summary>
        /// Clears all values from the cache
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }
    }
}
