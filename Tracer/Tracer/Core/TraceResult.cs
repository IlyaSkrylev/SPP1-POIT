using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tracer.Core
{
    public struct TraceResult
    {
        public List<ThreadInfoResult> ThreadInfoResults { get; set; }
    }
}
