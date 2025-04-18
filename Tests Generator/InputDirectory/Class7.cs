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

        public void GenerateReport()
        {
            Console.WriteLine($"Generating report: {_reportName}.");
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

        public void ScheduleJob(int jobId)
        {
            Console.WriteLine($"Scheduled job {jobId} with interval {_interval} minutes.");
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

        public void LogEvent(string eventDescription)
        {
            _logger.Log("Event: " + eventDescription);
        }
    }
}
