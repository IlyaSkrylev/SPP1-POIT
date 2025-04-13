using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker
{
    public class MyProgram
    {
        private class User
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public string GetData()
            {
                return Name + " " + Age; 
            }

        }

        public static void Main(string[] args)
        {

            var faker = new Faker.Faker();
            int i = faker.Create<int>(); // 542
            double d = faker.Create<double>(); // 12.458
            User user = faker.Create<User>(); // User { Name: "asdwerpasdf", Age: 987 }
                                              // Далее в примерах List носит иллюстративный характер. 
                                              // Вместо него может быть выбрана любая коллекция из условия.
            User user1 = faker.Create<User>(); // User { Name: "asdwerpasdf", Age: 987 }

            List<User> users = faker.Create<List<User>>();
            List<List<User>> lists = faker.Create<List<List<User>>>();
            List<int> ints = faker.Create<List<int>>();

            Console.WriteLine("int: " + i);
            Console.WriteLine("\ndouble: " + d);
            Console.WriteLine("\nUser: " + user.GetData());
            Console.WriteLine("\nUser: " + user1.GetData());
            Console.WriteLine("\nList<User>");
            foreach(User u in users)
            {
                Console.WriteLine(u.GetData() + "\n");
            }
            Console.WriteLine("\nList<List<User>>");
            foreach (var l in lists)
            {
                int iter = 1;
                foreach (var u in l)
                {
                    Console.WriteLine(iter + " " +u.GetData() + "\n");
                    iter++;
                }
            }
            Console.WriteLine("\nList<int>");
            foreach (int number in ints)
            {
                Console.WriteLine(number + "\n");
            }
        }
    }
}
