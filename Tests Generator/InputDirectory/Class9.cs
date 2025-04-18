using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 18. Класс UserProfile, конструктор принимает username (string) и age (int)
    public class UserProfile
    {
        private readonly string _username;
        private readonly int _age;

        public UserProfile(string username, int age)
        {
            _username = username;
            _age = age;
        }

        public void DisplayProfile()
        {
            Console.WriteLine($"User: {_username}, Age: {_age}");
        }
    }

    // 19. Класс SessionManager, конструктор принимает sessionId (string)
    public class SessionManager
    {
        private readonly string _sessionId;

        public SessionManager(string sessionId)
        {
            _sessionId = sessionId;
        }

        public void StartSession()
        {
            Console.WriteLine($"Session {_sessionId} started.");
        }
    }

    // 20. Класс AnalyticsEngine, конструктор принимает sampleSize (int)
    public class AnalyticsEngine
    {
        private readonly int _sampleSize;

        public AnalyticsEngine(int sampleSize)
        {
            _sampleSize = sampleSize;
        }

        public void RunAnalysis()
        {
            Console.WriteLine($"Running analysis with sample size {_sampleSize}.");
        }
    }
}
