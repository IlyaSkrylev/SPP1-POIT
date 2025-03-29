using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tracer.Core
{
    public struct ThreadInfoResult
    {
        [XmlAttribute("id")]
        public int ThreadId { get; set; }
        [XmlAttribute("time")]
        public int Time { get; set; }
        public List<MethodInfoResult> Methods { get; set; }
    }
}
