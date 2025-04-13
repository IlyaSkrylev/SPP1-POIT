using Faker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Generators
{
    public class StringGenerator : IValueGenerator
    {
        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public bool CanGenerate(Type type)
        {
            return type == typeof(string);
        }

        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            int length = context.Random.Next(5, 100);
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Characters[context.Random.Next(Characters.Length)];
            }
            return new string(result);
        }
    }
}
