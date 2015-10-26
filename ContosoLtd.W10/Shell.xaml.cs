using AppStudio.Common.Navigation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ContosoLtd;
using ContosoLtd.ViewModels;
using ContosoLtd.Views;

namespace ContosoLtd
{
    public sealed partial class Shell : Page
    {
        public Shell()
            : base()
        {
            this.ViewModel = new ShellViewModel();

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }

            App.Current.Suspending += Current_Suspending;

            this.Loaded += MainPage_Loaded;
        }
        
        public ShellViewModel ViewModel { get; set; }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.Initialize(typeof(App), MainFrame);
            NavigationService.NavigateToPage(typeof(HomePage));
        }

        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (NavigationService.CanGoBack())
            {
                NavigationService.GoBack();
                e.Handled = true;
            }
        }
    }
}
