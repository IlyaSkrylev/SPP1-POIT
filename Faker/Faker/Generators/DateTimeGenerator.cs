using Faker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Generators
{
    public class DateTimeGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type)
        {
            return type == typeof(DateTime);
        }

        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            var start = new DateTime(1970, 1, 1);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(context.Random.Next(range));
        }
    }
}
