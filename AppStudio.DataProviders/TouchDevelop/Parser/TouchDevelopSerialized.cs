using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.TouchDevelop.Parser
{
    internal class TouchDevelopSerialized
    {
        public DateTime TimeStamp { get; set; }
        public IEnumerable<TouchDevelopSchema> Items { get; set; }
    }
}
