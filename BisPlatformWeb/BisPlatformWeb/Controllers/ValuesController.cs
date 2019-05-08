using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BisInterface;
using BisPlatform.Data.Entity;
using BisPlatformWeb.Common;
using BisPlatformWeb.Common.Redis;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace BisPlatformWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ITestService _testInterface;
        private IMemoryCache _cache;
        IDistributedCache _distributedCache;
        public ValuesController(ITestService testInterface, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _testInterface = testInterface;
            _cache = memoryCache;
            _distributedCache = distributedCache;
        }
        // GET api/values
        [HttpGet, Log("测试get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            var domain222 = _testInterface.add(1, 2);
            #region redis
            _distributedCache.SetString("name", "zhangsan");
            var value = _distributedCache.GetString("name");
            #endregion
            var domaincache = _cache.Get("getkey");
            if (domaincache == null)
            {
                var domain = _testInterface.add(1, 2);
                _cache.Set("getkey", domain);
            }
            //var aa = "dfasdfas";
            //int bb = Convert.ToInt32(aa);
            NlogHelper.InfoLog("1234123");

            return new string[] { "value1", "value2" };
        }
        [HttpGet,Route("get"),Log("测试get")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost, Route("GetValue"), Log("测试GetValue")]
        public ActionResult<string> GetValue(TestEntity test)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
