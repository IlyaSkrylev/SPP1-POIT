using MyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 3. Класс DataProcessor, конструктор принимает ILogger
    public class DataProcessor
    {
        private readonly ILogger _logger;

        public DataProcessor(ILogger logger)
        {
            _logger = logger;
        }

        public void ProcessData(int data)
        {
            _logger.Log("Processing data: " + data);
        }
    }

    // 4. Класс ServiceA, конструктор принимает IService
    public class ServiceA
    {
        private readonly IService _service;

        public ServiceA(IService service)
        {
            _service = service;
        }

        public void ExecuteService(int value)
        {
            Console.WriteLine("Executing service with value " + value);
            _service.Serve();
        }
    }
}
