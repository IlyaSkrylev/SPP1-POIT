using Faker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Generators
{
    public class DoubleGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type)
        {
            return type == typeof(double);
        }

        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            return (context.Random.Next(0, 2) == 0 ? 1 : -1) * context.Random.NextDouble() * context.Random.Next(int.MinValue, int.MaxValue);
        }
    }
}
