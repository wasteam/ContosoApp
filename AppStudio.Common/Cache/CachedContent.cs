using System;
using System.Collections.Generic;

namespace AppStudio.Common.Cache
{
    public class CachedContent<T>
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
