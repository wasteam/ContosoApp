using System;
using System.Windows.Input;
using AppStudio.Common;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using ContosoLtd.Navigation;

namespace ContosoLtd.ViewModels
{
    public class ShellViewModel : ObservableBase
    {
        private AppNavigation _navigation;
        private bool _navPanelOpened;
        private string _appTitle;

        public ShellViewModel()
        {
            Navigation = new AppNavigation();
            Navigation.LoadNavigation();

            NavigationService.NavigatedToPage += NavigationService_NavigatedToPage;
        }

        public AppNavigation Navigation
        {
            get { return _navigation; }
            set { SetProperty(ref _navigation, value); }
        }

        public bool NavPanelOpened
        {
            get { return _navPanelOpened; }
            set { SetProperty(ref _navPanelOpened, value); }
        }

        public string AppTitle
        {
            get { return _appTitle; }
            set { SetProperty(ref _appTitle, value); }
        }

        public ICommand ItemSelected
        {
            get
            {
                return new RelayCommand<NavigationNode>(n =>
                {
                    n.Selected();
                });
            }
        }

        public ICommand NavPanelClick
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NavPanelOpened = !NavPanelOpened;
                });
            }
        }

        public ICommand GoBackCommand
        {
            get
            {
                return NavigationService.GoBackCommand;
            }
        }

        private void NavigationService_NavigatedToPage(Type page, object parameter)
        {
            var navigatedNode = Navigation.FindPage(page);
            if (navigatedNode != null)
            {
                if (!string.IsNullOrEmpty(navigatedNode.Title))
                {
                    AppTitle = navigatedNode.Title;
                }
                else
                {
                    AppTitle = navigatedNode.Label;
                }
                Navigation.Active = navigatedNode;
            }
            else
            {
                AppTitle = string.Empty;
                Navigation.Active = null;
            }

            if (NavPanelOpened)
            {
                NavPanelOpened = false;
            }
            OnPropertyChanged("GoBackCommand");
        }
    }
}
