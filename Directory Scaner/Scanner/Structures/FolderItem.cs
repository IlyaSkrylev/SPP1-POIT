using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Structures
{
    public class FolderItem
    {
        public string name {  get; set; }
        public long size { get; set; }
        public double percentage { get; set; }
        public List<FolderItem> folders { get; set; }
        public List<FileItem> files { get; set; }
    }
}
