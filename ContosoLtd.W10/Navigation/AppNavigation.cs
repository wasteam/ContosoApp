using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppStudio.Common.Navigation;
using Windows.UI.Xaml;

namespace ContosoLtd.Navigation
{
    public class AppNavigation
    {
        private NavigationNode _active;

        static AppNavigation()
        {

        }

        public NavigationNode Active
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != null)
                {
                    _active.IsSelected = false;
                }
                _active = value;
                if (_active != null)
                {
                    _active.IsSelected = true;
                }
            }
        }


        public ObservableCollection<NavigationNode> Nodes { get; private set; }

        public void LoadNavigation()
        {
            Nodes = new ObservableCollection<NavigationNode>();

            Nodes.Add(new ItemNavigationNode
            {
                Title = @"Contoso Ltd",
                Label = "Home",
                IsSelected = true,
                NavigationInfo = NavigationInfo.FromPage("HomePage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "About us",
                NavigationInfo = NavigationInfo.FromPage("AboutUsListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "Catalog",
                NavigationInfo = NavigationInfo.FromPage("CatalogListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "Team",
                NavigationInfo = NavigationInfo.FromPage("TeamListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "News",
                NavigationInfo = NavigationInfo.FromPage("NewsListPage")
            });

            Nodes.Add(new ItemNavigationNode
            {
                Label = "About",
                NavigationInfo = NavigationInfo.FromPage("AboutPage")
            });
        }

        public NavigationNode FindPage(Type pageType)
        {
            return GetAllItemNodes(Nodes).FirstOrDefault(n => n.NavigationInfo.Type == NavigationType.Page && n.NavigationInfo.TargetPage == pageType.Name);
        }

        private IEnumerable<ItemNavigationNode> GetAllItemNodes(IEnumerable<NavigationNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is ItemNavigationNode)
                {
                    yield return node as ItemNavigationNode;
                }
                else if(node is GroupNavigationNode)
                {
                    var gNode = node as GroupNavigationNode;

                    foreach (var innerNode in GetAllItemNodes(gNode.Nodes))
                    {
                        yield return innerNode;
                    }
                }
            }
        }
    }
}
