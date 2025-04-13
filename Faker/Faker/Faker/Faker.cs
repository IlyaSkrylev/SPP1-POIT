using Faker.Generators;
using Faker.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Faker.Faker
{
    public class Faker : IFaker
    {
        private readonly List<IValueGenerator> _generators;
        private readonly Random _random;
        private readonly Dictionary<Type, object> _generatedObjectsCache;

        public Faker()
        {
            _random = new Random();
            _generatedObjectsCache = new Dictionary<Type, object>();

            _generators = new List<IValueGenerator>
            {
                new IntGenerator(),
                new DoubleGenerator(),
                new StringGenerator(),
                new DateTimeGenerator(),
                new ListGenerator(),
                new ObjectGenerator()
            }; 
        }

        public object CreateAndRegisterEmptyObject(Type type)
        {
            if (_generatedObjectsCache.TryGetValue(type, out var existingObj))
                return existingObj;

            var emptyObj = FormatterServices.GetUninitializedObject(type);
            _generatedObjectsCache[type] = emptyObj;

            return emptyObj;
        }

        public T Create<T>() => (T)Create(typeof(T));

        public object Create(Type type)
        {

            var generator = _generators.FirstOrDefault(g => g.CanGenerate(type))
                ?? throw new InvalidOperationException($"No generator for type {type}");

            var context = new GeneratorContext(_random, this);
            return generator.Generate(type, context);
        }

        public void ClearObj()
        {
            _generatedObjectsCache.Clear();
        }
    }
}