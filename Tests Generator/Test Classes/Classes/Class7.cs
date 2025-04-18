using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 14. Класс ReportGenerator, конструктор принимает reportName (string)
    public class ReportGenerator
    {
        private readonly string _reportName;

        public ReportGenerator(string reportName)
        {
            _reportName = reportName;
        }

        public double GenerateReport()
        {
            Console.WriteLine($"Generating report: {_reportName}.");
            return 5.6;
        }
    }

    // 15. Класс Scheduler, конструктор принимает interval (int)
    public class Scheduler
    {
        private readonly int _interval;

        public Scheduler(int interval)
        {
            _interval = interval;
        }

        public string ScheduleJob(int jobId)
        {
            Console.WriteLine($"Scheduled job {jobId} with interval {_interval} minutes.");
            return "hello world";
        }
    }

    // 16. Класс EventLogger, конструктор принимает ILogger
    public class EventLogger
    {
        private readonly ILogger _logger;

        public EventLogger(ILogger logger)
        {
            _logger = logger;
        }

        public bool LogEvent(string eventDescription)
        {
            _logger.Log("Event: " + eventDescription);
            return false;
        }
    }
}
