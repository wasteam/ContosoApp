using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;
using AppStudio.Common;

using ContosoLtd;
using ContosoLtd.Sections;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Views
{
    public sealed partial class TeamDetailPage : PageBase
    {
        private DataTransferManager _dataTransferManager;

        public TeamDetailPage()
        {
            this.ViewModel = new DetailViewModel<Team1Schema>(new TeamConfig());            
            this.InitializeComponent();
        }

        public DetailViewModel<Team1Schema> ViewModel { get; set; }        

        protected async override void LoadState(object navParameter)
        {
            await this.ViewModel.LoadDataAsync(navParameter as ItemViewModel);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }
    }
}
