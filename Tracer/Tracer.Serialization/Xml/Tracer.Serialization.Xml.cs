using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization
{
    public class XmlTraceResultSerializer : ITraceResultSerializer
    {
        public string Format => "xml";

        public void Serialize(TraceResult traceResult, Stream to)
        {
            var serializer = new XmlSerializer(typeof(TraceResult));
            serializer.Serialize(to, traceResult);
        }
    }
}
