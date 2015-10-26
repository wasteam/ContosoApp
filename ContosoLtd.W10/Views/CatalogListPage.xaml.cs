using Windows.UI.Xaml.Navigation;
using AppStudio.Common;

using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class CatalogListPage : PageBase
    {
        public CatalogListPage()
        {
            this.ViewModel = new ListViewModel<Catalog1Schema>(new CatalogConfig());
            this.InitializeComponent();
        }

        public ListViewModel<Catalog1Schema> ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
