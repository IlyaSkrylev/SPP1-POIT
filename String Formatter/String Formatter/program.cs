using StringFormatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace String_Formatting
{
    class User
    {
        public string FirstName { get; }
        public string LastName { get; }
        public List<string> Orders { get; }

        public User(string fn, string ln, List<string> orders)
        {
            FirstName = fn;
            LastName = ln;
            Orders = orders;
        }

        public string Greet() =>
            StringFormatter.Shared.Format(
                "{FirstName} {LastName} заказал(-а) {Orders[0]}", this);

        public string escapingParentheses() =>
            StringFormatter.Shared.Format(
                "{{FirstName}} транслируется в {FirstName}", this);
    }
    internal class program
    {
        public static void Main(string[] args)
        {
            try
            {
                var user = new User("Петя", "Иванов", new List<string> { "утюг" });
                Console.WriteLine(user.Greet());
                Console.WriteLine(user.escapingParentheses());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
