using AppTodo.Models;
using AppTodo.RedisCache;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppTodo.Controllers
{
    [RoutePrefix("api/todo")]
    public class TodoController : ApiController
    {
        private readonly IDatabase redis;
        private readonly IServer server;
        private readonly RedisConnection obj = new RedisConnection();
        public TodoController()
        {
            redis = obj.Connection.GetDatabase();
            server = obj.Connection.GetServer("Redisv1.redis.cache.windows.net:6380");
        }

        [HttpGet]
        [Route("list")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            List<Todo> todos = new List<Todo>();
            try
            {
                foreach (var key in server.Keys(pattern: "*Key-*"))
                    foreach (var val in await redis.HashValuesAsync(key))
                        todos.Add(JsonConvert.DeserializeObject<Todo>(val.ToString()));
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(todos.OrderByDescending(r=>r.Timestamp));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> CreateAsync(Todo model)
        {
            try
            {
                var hashKey = $"Key-{Guid.NewGuid()}";
                model.Key = hashKey;
                model.Timestamp = DateTime.UtcNow;

                HashEntry[] redisHash = {
                new HashEntry(Guid.NewGuid().ToString(), JsonConvert.SerializeObject(model))
                };

                await redis.HashSetAsync(hashKey, redisHash);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{key}/remove")]
        public async Task<IHttpActionResult> RemoveByIdAsync(string key)
        {
            try
            {
                await redis.KeyDeleteAsync(key);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            return Ok();
        }
    }
}
