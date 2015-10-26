using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppStudio.Common;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.Bing;
using AppStudio.DataProviders.Menu;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.DynamicStorage;
using ContosoLtd.Sections;


namespace ContosoLtd.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel(int visibleItems)
        {
            this.AboutUs = new ListViewModel<HtmlSchema>(new AboutUsConfig(), visibleItems);
            this.Catalog = new ListViewModel<Catalog1Schema>(new CatalogConfig(), visibleItems);
            this.Team = new ListViewModel<Team1Schema>(new TeamConfig(), visibleItems);
            this.News = new ListViewModel<BingSchema>(new NewsConfig(), visibleItems);
            this.ContactUs = new ListViewModel<MenuSchema>(new ContactUsConfig());
            Actions = new List<ActionInfo>();

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = new RelayCommand(Refresh),
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    Type = ActionType.Primary
                });
            }
        }

        public ListViewModel<HtmlSchema> AboutUs { get; private set; }
        public ListViewModel<Catalog1Schema> Catalog { get; private set; }
        public ListViewModel<Team1Schema> Team { get; private set; }
        public ListViewModel<BingSchema> News { get; private set; }
        public ListViewModel<MenuSchema> ContactUs { get; private set; }

        public RelayCommand<INavigable> SectionHeaderClickCommand
        {
            get
            {
                return new RelayCommand<INavigable>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }

        public string LastUpdated
        {
            get 
            { 
                return GetViewModels().Select(vm => vm.LastUpdated)
                            .OrderByDescending(d => d).FirstOrDefault(); 
            }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private async void Refresh()
        {
            var refreshDataTasks = GetViewModels()
                                        .Where(vm => !vm.HasLocalData)
                                        .Select(vm => vm.LoadDataAsync(true));

            await Task.WhenAll(refreshDataTasks);

            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<DataViewModelBase> GetViewModels()
        {
            yield return AboutUs;
            yield return Catalog;
            yield return Team;
            yield return News;
            yield return ContactUs;
        }
    }
}
