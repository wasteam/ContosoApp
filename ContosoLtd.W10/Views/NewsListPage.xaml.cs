using Windows.UI.Xaml.Navigation;
using AppStudio.Common;
using AppStudio.DataProviders.Bing;
using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class NewsListPage : PageBase
    {
        public NewsListPage()
        {
            this.ViewModel = new ListViewModel<BingSchema>(new NewsConfig());
            this.InitializeComponent();
        }

        public ListViewModel<BingSchema> ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
