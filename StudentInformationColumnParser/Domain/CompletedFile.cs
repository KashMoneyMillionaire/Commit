using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitParser.Domain
{
    public class CompletedFile : EntityBase<long>
    {
        public string FileName { get; set; }
        public DateTime TimeCompleted { get; set; }
        public bool IsCompleted { get; set; }
    }
}
