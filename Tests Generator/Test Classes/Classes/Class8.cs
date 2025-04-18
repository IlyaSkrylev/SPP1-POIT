using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 17. Класс SecurityManager, конструктор принимает bool и string
    public class SecurityManager
    {
        private readonly bool _enableEncryption;
        private readonly string _secretKey;

        public SecurityManager(bool enableEncryption, string secretKey)
        {
            _enableEncryption = enableEncryption;
            _secretKey = secretKey;
        }

        public void Authenticate(string username, string password)
        {
            Console.WriteLine($"Authenticating {username} with encryption {_enableEncryption}.");
        }
    }
}
