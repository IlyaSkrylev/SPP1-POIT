using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization
{
    public class TraceResultSerializer
    {
        private readonly string _pluginPath;

        public TraceResultSerializer(string pluginPath)
        {
            _pluginPath = pluginPath;
        }

        public IEnumerable<ITraceResultSerializer> LoadSerializers()
        {
            var serializerList = new List<ITraceResultSerializer>();
            var dllFiles = Directory.GetFiles(_pluginPath, "*.dll");

            foreach (var dll in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dll);
                try
                {
                    var types = assembly.GetTypes()
                        .Where(t => typeof(ITraceResultSerializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                    foreach (var type in types)
                    {
                        var serializer = (ITraceResultSerializer)Activator.CreateInstance(type);
                        serializerList.Add(serializer);
                    }
                }
                catch { }
            }
            return serializerList;
        }
    }
}
