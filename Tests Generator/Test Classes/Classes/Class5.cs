using MyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 9. Класс ConfigManager с конструктором без параметров
    public class ConfigManager
    {
        public ConfigManager() { }

        public void LoadConfig()
        {
            Console.WriteLine("Configuration loaded.");
        }
    }

    // 10. Класс NotificationService, конструктор принимает IEmailSender
    public class NotificationService
    {
        private readonly IEmailSender _emailSender;

        public NotificationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Notify(string email, string subject)
        {
            _emailSender.SendEmail(email, subject, "This is a notification.");
        }
    }
}
