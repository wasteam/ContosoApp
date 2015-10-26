using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Menu;
using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class ContactUsListPage : PageBase
    {
        public ContactUsListPage()
        {
            this.ViewModel = new ListViewModel<MenuSchema>(new ContactUsConfig());
            this.InitializeComponent();
        }

        public ListViewModel<MenuSchema> ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
