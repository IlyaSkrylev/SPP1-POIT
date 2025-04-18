using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 8. Класс FileHandler с параметрами filePath и bufferSize
    public class FileHandler
    {
        private readonly string _filePath;
        private readonly int _bufferSize;

        public FileHandler(string filePath, int bufferSize)
        {
            _filePath = filePath;
            _bufferSize = bufferSize;
        }

        public void ReadFile()
        {
            Console.WriteLine($"Reading file {_filePath} with buffer size {_bufferSize}");
        }
    }
}
