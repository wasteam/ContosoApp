using System;
using AppStudio.DataProviders;

namespace ContosoLtd.Sections
{
    /// <summary>
    /// Implementation of the Catalog1Schema class.
    /// </summary>
    public class Catalog1Schema : SchemaBase
    {

        public string Image { get; set; }

        public string Name { get; set; }

        public string Reference { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public string MoreInfo { get; set; }
    }
}
