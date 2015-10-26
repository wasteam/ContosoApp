using Windows.UI.Xaml.Navigation;
using AppStudio.Common;

using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class TeamListPage : PageBase
    {
        public TeamListPage()
        {
            this.ViewModel = new ListViewModel<Team1Schema>(new TeamConfig());
            this.InitializeComponent();
        }

        public ListViewModel<Team1Schema> ViewModel { get; set; }

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync();
        }

    }
}
