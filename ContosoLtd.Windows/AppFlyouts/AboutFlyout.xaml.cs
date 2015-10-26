using ContosoLtd.ViewModels;
using Windows.UI.Xaml.Controls;

namespace ContosoLtd.AppFlyouts
{
    public sealed partial class AboutFlyout : SettingsFlyout
    {
        public AboutThisAppViewModel AboutThisAppModel { get; private set; }
        public AboutFlyout()
        {
            AboutThisAppModel = new AboutThisAppViewModel();
            this.DataContext = this;
            this.InitializeComponent();
        }
    }
}
