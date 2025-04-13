using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core;
using Tracer.Serialization.Abstractions;
using YamlDotNet.Serialization;

namespace Tracer.Serialization
{
    public class YamlTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => "yaml";

        public void Serialize(TraceResult traceResult, Stream to)
        {
            var serializer = new SerializerBuilder().Build();
            using (var writer = new StreamWriter(to))
            {
                string yaml = serializer.Serialize(traceResult);
                writer.Write(yaml);
            }
        }
    }
}
