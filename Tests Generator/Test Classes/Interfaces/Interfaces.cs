namespace MyApp
{
    public interface ILogger
    {
        void Log(string message);
    }

    public interface IService
    {
        void Serve();
    }

    public interface IEmailSender
    {
        void SendEmail(string to, string subject, string body);
    }

    public interface IDataRepository
    {
        void Save(string data);
    }
}