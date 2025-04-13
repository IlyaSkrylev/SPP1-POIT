using Faker.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Faker.Generators
{
    public class ListGenerator : IValueGenerator
    {
        public bool CanGenerate(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            var elementType = typeToGenerate.GetGenericArguments()[0];
            var listInstance = (IList)Activator.CreateInstance(typeToGenerate);

            int randomSize = context.Random.Next(2, 5);
            for (int i = 0; i < randomSize; i++)
            {
                context.Faker.ClearObj();
               
                var element = context.Faker.Create(elementType);
                listInstance.Add(element);
            }

            return listInstance;
        }
    }
}