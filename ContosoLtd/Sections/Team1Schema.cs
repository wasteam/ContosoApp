using System;
using AppStudio.DataProviders;

namespace ContosoLtd.Sections
{
    /// <summary>
    /// Implementation of the Team1Schema class.
    /// </summary>
    public class Team1Schema : SchemaBase
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string JobTitle { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public string Image { get; set; }

        public string OfficeLocation { get; set; }
    }
}
