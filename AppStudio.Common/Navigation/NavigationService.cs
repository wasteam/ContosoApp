using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using AppStudio.Common.Commands;

namespace AppStudio.Common.Navigation
{
    public delegate void NavigationEventHandler(Type page, object parameter);

    public class NavigationService
    {
        private static Assembly _appAssembly;
        private static Frame _rootFrame;

        public static event NavigationEventHandler NavigatedToPage;

        public static void Initialize(Type app, Frame rootFrame)
        {
            _appAssembly = app.GetTypeInfo().Assembly;
            _rootFrame = rootFrame;
        }

        public static void NavigateToPage(Type page, object parameter = null)
        {
            if (IsInitialized() && page != null)
            {
                _rootFrame.Navigate(page, parameter);

                if (NavigatedToPage != null)
                {
                    NavigatedToPage(page, parameter);
                }
            }
        }

        public static void NavigateToPage(string page, object parameter = null)
        {
            var targetPage = _appAssembly.DefinedTypes.FirstOrDefault(t => t.Name == page);

            if (targetPage != null)
            {
                NavigateToPage(targetPage.AsType(), parameter);
            }
        }

        public static async Task NavigateTo(Uri uri)
        {
            if (uri != null)
            {
                await Launcher.LaunchUriAsync(uri);
            }

        }

        public static void NavigateTo(INavigable item)
        {
            if (item != null && item.NavigationInfo != null)
            {
                if (item.NavigationInfo.Type == NavigationType.Page)
                {
                    var navParam = item.NavigationInfo.IncludeState ? item : null;

                    NavigationService.NavigateToPage(item.NavigationInfo.TargetPage, navParam);
                }
                else if (item.NavigationInfo.Type == NavigationType.DeepLink)
                {
                    NavigationService.NavigateTo(item.NavigationInfo.TargetUri).FireAndForget();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("NavigationInfo.Type");
                }
            }
        }

        public static RelayCommand GoBackCommand
        {
            get
            {
                return new RelayCommand(
                        () => GoBack(),
                        () => CanGoBack());
            }
        }

        public static bool CanGoBack()
        {
            return IsInitialized() && _rootFrame.CanGoBack;
        }

        public static void GoBack()
        {
            if (CanGoBack())
            {
                var targetBackStack = _rootFrame.BackStack.Last();

                _rootFrame.GoBack();

                if (targetBackStack != null && NavigatedToPage != null)
                {
                    NavigatedToPage(targetBackStack.SourcePageType, targetBackStack.Parameter);
                }
            }
        }

        public static RelayCommand GoForwardCommand
        {
            get
            {
                return new RelayCommand(
                        () => GoForward(),
                        () => CanGoForward());
            }
        }

        public static bool CanGoForward()
        {
            return IsInitialized() && _rootFrame.CanGoForward;
        }

        public static void GoForward()
        {
            if (CanGoForward())
            {
                _rootFrame.GoForward();
            }
        }

        private static bool IsInitialized()
        {
            return _rootFrame != null;
        }
    }
}
