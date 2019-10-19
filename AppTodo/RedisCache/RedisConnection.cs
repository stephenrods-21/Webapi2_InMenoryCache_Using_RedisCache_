using StackExchange.Redis;
using System;

namespace AppTodo.RedisCache
{
    public class RedisConnection
    {
        static RedisConnection()
        {
            RedisConnection.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("Redisv1.redis.cache.windows.net:6380,password=1SzCbcLh7wwsDsDYnh1YtWmUK+mk6o2lrdZVkb4WFR0=,ssl=True,abortConnect=False");
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}