using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.DataProviders.Menu
{
    public class MenuSchema : SchemaBase
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Target { get; set; }
    }
}
