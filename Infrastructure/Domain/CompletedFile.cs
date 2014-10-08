using System;

namespace Infrastructure.Domain
{
    public class CompletedFile : EntityBase<long>
    {
        public string FileName { get; set; }
        public DateTime TimeCompleted { get; set; }
        public bool IsCompleted { get; set; }
    }
}
