using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ContosoLtd.ViewModels
{
    public class TouchDevelopItemDesignViewModel
    {
        public string IconUrl { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string CumulativePositiveReviews { get; set; }
        public bool HasScreenshot { get; set; }
        public string ScreenshotUrl { get; set; }
        public string Description { get; set; }
    }
    public class TouchDevelopDesignViewModel
    {
        public ObservableCollection<TouchDevelopItemDesignViewModel> Items { get; set; }
    }
}
