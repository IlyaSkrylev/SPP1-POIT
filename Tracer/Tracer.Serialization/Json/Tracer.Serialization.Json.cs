using System.IO;
using System.Text.Json;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization
{
    public class JsonTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => "json";

        public void Serialize(TraceResult traceResult, Stream to)
        {
            using (var writer = new StreamWriter(to))
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(traceResult, options);
                writer.Write(json);
            }
        }
    }
}
