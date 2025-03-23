//using Auth.WebAPI.Helper;
//using Auth.WebAPI.Manager.Interface.Common;
//using StackExchange.Redis;
//using System.Reflection;
//using System.Text.Json;

//namespace Auth.WebAPI.Manager.Implementation.Common
//{
//    public class CacheManager : ICacheManager
//    {
//        private readonly IConnectionMultiplexer _connectionMultiplexer;
//        public CacheManager(IConnectionMultiplexer connectionMultiplexer)
//        {
//            _connectionMultiplexer = connectionMultiplexer;
//        }
//        public async Task<T> GetData<T>(string key)
//        {
//            var db = _connectionMultiplexer.GetDatabase();
//            var value = await db.StringGetAsync(key);

//            if (!string.IsNullOrEmpty(value))
//                return JsonSerializer.Deserialize<T>(value!);

//            return default;
//        }

//        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
//        {
//            var _cacheDb = _connectionMultiplexer.GetDatabase();
//            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

//            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
//        }

//        public object RemoveData(string key)
//        {
//            var _cacheDb = _connectionMultiplexer.GetDatabase();

//            var _exist = _cacheDb.KeyExists(key);

//            if (_exist)
//                return _cacheDb.KeyDelete(key);

//            return false;
//        }

//        public async Task RemoveAllCommonPropertyCachedKeys()
//        {
//            var redisKeys = typeof(B2CAuthRedisKey)
//                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
//                .Where(fi => fi.IsLiteral && fi.FieldType == typeof(string) && !string.IsNullOrEmpty(fi.GetRawConstantValue() as string))
//                .Select(fi => new RedisKey(fi.GetRawConstantValue() as string))
//                .ToArray();

//            var cacheDb = _connectionMultiplexer.GetDatabase();

//            foreach (var key in redisKeys)
//            {
//                await cacheDb.KeyDeleteAsync(key, CommandFlags.FireAndForget | CommandFlags.DemandMaster);
//            }
//        }
//    }
//}