using Windows.UI.Xaml.Navigation;
using AppStudio.Common;

using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class CatalogListPage : PageBase
    {
        public ListViewModel<Catalog1Schema> ViewModel { get; set; }

        public CatalogListPage()
        {
            this.ViewModel = new ListViewModel<Catalog1Schema>(new CatalogConfig());
            this.InitializeComponent();
        }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
