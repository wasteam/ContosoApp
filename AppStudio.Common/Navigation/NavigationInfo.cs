using System;
using AppStudio.DataProviders.Menu;

namespace AppStudio.Common.Navigation
{
    public class NavigationInfo
    {
        public string TargetPage { get; set; }
        
        public Uri TargetUri { get; set; }

        public NavigationType Type { get; set; }

        public bool IncludeState { get; set; }

        public static NavigationInfo FromMenu(MenuSchema menuItem, bool includeState = false)
        {
            var navigationInfo = new NavigationInfo
            {
                Type = SafeParse(menuItem.Type),
                IncludeState = includeState
            };

            if (navigationInfo.Type == NavigationType.Page)
            {
                navigationInfo.TargetPage = menuItem.Target;
            }
            else
            {
                navigationInfo.TargetUri = new Uri(menuItem.Target, UriKind.Absolute);
            }

            return navigationInfo;
        }

        public static NavigationInfo FromPage(string pageFullType, bool includeState = false)
        {
            return new NavigationInfo
            {
                Type = NavigationType.Page,
                TargetPage = pageFullType,
                IncludeState = includeState
            };
        }

        private static NavigationType SafeParse(string value)
        {
            var type = NavigationType.Page;
            Enum.TryParse(value, out type);

            return type;
        }
    }

    public enum NavigationType
    {
        Page,
        DeepLink
    }
}
