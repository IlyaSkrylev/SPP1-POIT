using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faker.Generators
{
    using global::Faker.Faker;
    using global::Faker.Interfaces;
    using System;
    using System.Linq;
    using System.Reflection;

    public class ObjectGenerator : IValueGenerator
    {
        private readonly HashSet<object> _creatingObjects = new HashSet<object>();
        public bool CanGenerate(Type type)
        {
            return !type.IsPrimitive && type != typeof(string) && !IsCollectionType(type) && !type.IsAbstract;
        }

        public object Generate(Type typeToGenerate, GeneratorContext context)
        {
            if (_creatingObjects.Contains(context.Faker.CreateAndRegisterEmptyObject(typeToGenerate)))
            {
                return null;
            }

            _creatingObjects.Add(context.Faker.CreateAndRegisterEmptyObject(typeToGenerate));

            try
            {
                if (typeToGenerate.IsValueType)
                {
                    return GenerateValueType(typeToGenerate, context);
                }
                else
                {
                    return GenerateReferenceType(typeToGenerate, context);
                }
            }
            finally
            {
                if (_creatingObjects.Contains(context.Faker.CreateAndRegisterEmptyObject(typeToGenerate)))
                {
                    _creatingObjects.Remove(context.Faker.CreateAndRegisterEmptyObject(typeToGenerate));
                }
            }
        }

        private object GenerateValueType(Type type, GeneratorContext context)
        {
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .ToList();

            if (constructors.Count > 0)
            {
                foreach (var constructor in constructors)
                {
                    try
                    {
                        var parameters = constructor.GetParameters()
                            .Select(p => context.Faker.Create(p.ParameterType))
                            .ToArray();
                        var instance = constructor.Invoke(parameters);
                        FillPropertiesAndFields(instance, type, context);
                        return instance;
                    }
                    catch
                    {
                    }
                }
            }

            var defaultInstance = Activator.CreateInstance(type);
            FillPropertiesAndFields(defaultInstance, type, context);
            return defaultInstance;
        }

        private object GenerateReferenceType(Type type, GeneratorContext context)
        {
            var obj = context.Faker.CreateAndRegisterEmptyObject(type);
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .ToList();

            if (constructors.Count == 0)
            {
                throw new InvalidOperationException($"No public constructors found for type {type.FullName}");
            }

            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters()
                        .Select(p => context.Faker.Create(p.ParameterType))
                        .ToArray();
                    var instance = constructor.Invoke(parameters);
                    FillPropertiesAndFields(instance, type, context);
                    return instance;
                }
                catch
                {
                }
            }

            throw new InvalidOperationException($"Failed to create instance of type {type.FullName}");
        }

        private void FillPropertiesAndFields(object instance, Type type, GeneratorContext context)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite);
            foreach (var property in properties)
            {
                var value = context.Faker.Create(property.PropertyType);
                property.SetValue(instance, value);
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = context.Faker.Create(field.FieldType);
                field.SetValue(instance, value);
            }
        }

        private bool IsCollectionType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }
    }
}
