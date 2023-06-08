using Common.Cache;
using POCO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ApplicationCache
{
    public class StudentCache
    {
        private readonly RedisCacheManager _cacheManager;

        public StudentCache(RedisCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void SaveStudentsToCache(string key, List<Student> students, TimeSpan? expirationTime = null)
        {
            _cacheManager.SaveDataToCache(key, students, expirationTime);
        }

        public List<Student> GetStudentsFromCache(string key)
        {
            return _cacheManager.GetDataFromCache<List<Student>>(key);
        }

        public void ClearCache(string key)
        {
            _cacheManager.ClearCache(key);
        }
    }
}
