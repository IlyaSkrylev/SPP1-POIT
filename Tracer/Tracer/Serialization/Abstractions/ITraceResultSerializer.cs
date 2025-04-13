using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracer.Core;

namespace Tracer.Serialization.Abstractions
{
    public interface ITraceResultSerializer
    {
        // Опционально: возвращает формат, используемый сериализатором (xml/json/yaml).
        // Может быть удобно для выбора имени файлов (см. ниже).
        string Format { get; }
        void Serialize(TraceResult traceResult, Stream to);
    }
}
