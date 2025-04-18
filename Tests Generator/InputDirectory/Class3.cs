using MyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 5. Класс CacheManager с конструктором, принимающим string и int
    public class CacheManager
    {
        private readonly string _connectionString;
        private readonly int _cacheSize;

        public CacheManager(string connectionString, int cacheSize)
        {
            _connectionString = connectionString;
            _cacheSize = cacheSize;
        }

        public void AddToCache(string key, string value)
        {
            Console.WriteLine($"Added {key}:{value} to cache.");
        }
    }

    // 6. Класс UserManager, конструктор принимает IDataRepository
    public class UserManager
    {
        private readonly IDataRepository _repository;

        public UserManager(IDataRepository repository)
        {
            _repository = repository;
        }

        public void CreateUser(string username)
        {
            Console.WriteLine($"User {username} created.");
            _repository.Save(username);
        }
    }

    // 7. Класс Repository, реализующий IDataRepository
    public class Repository : IDataRepository
    {
        public Repository() { }

        public void Save(string data)
        {
            Console.WriteLine($"Data '{data}' saved to repository.");
        }
    }
}
