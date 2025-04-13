using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Interfaces
{
    public interface IFaker
    {
        T Create<T>();
        object Create(Type type);
        object CreateAndRegisterEmptyObject(Type type); void ClearObj();
    }

    public interface IValueGenerator
    {
        object Generate(Type typeToGenerate, GeneratorContext context);
        bool CanGenerate(Type type);
    }

    public class GeneratorContext
    {
        public Random Random { get; }
        public IFaker Faker { get; }

        public GeneratorContext(Random random, IFaker faker)
        {
            Random = random;
            Faker = faker;
        }
    }
}
