using MyApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    // 11. Класс EmailSender, реализующий IEmailSender
    public class EmailSender : IEmailSender
    {
        public EmailSender() { }

        public void SendEmail(string to, string subject, string body)
        {
            Console.WriteLine($"Email sent to {to} with subject '{subject}'.");
        }
    }

    // 12. Класс OrderProcessor, конструктор принимает int и IService
    public class OrderProcessor
    {
        private readonly int _orderId;
        private readonly IService _service;

        public OrderProcessor(int orderId, IService service)
        {
            _orderId = orderId;
            _service = service;
        }

        public void ProcessOrder()
        {
            Console.WriteLine($"Processing order #{_orderId}.");
            _service.Serve();
        }
    }

    // 13. Класс PaymentProcessor, конструктор принимает bool
    public class PaymentProcessor
    {
        private readonly bool _useSandbox;

        public PaymentProcessor(bool useSandbox)
        {
            _useSandbox = useSandbox;
        }

        public void ProcessPayment(int amount)
        {
            Console.WriteLine($"Processing payment of {amount} using {(_useSandbox ? "sandbox" : "production")} mode.");
        }
    }
}
