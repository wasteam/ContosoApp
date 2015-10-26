using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AppStudio.Common.Actions;
using AppStudio.Common.Cache;
using AppStudio.Common.Commands;
using AppStudio.Common.DataSync;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using Windows.ApplicationModel.DataTransfer;
using ContosoLtd.Config;

namespace ContosoLtd.ViewModels
{
    public class ListViewModel<T> : DataViewModelBase<T>, INavigable where T : SchemaBase
    {
        private int _visibleItems;
        private bool _hasMoreItems;
        private ObservableCollection<ItemViewModel> _items;
        private SectionConfigBase<T> _sectionConfig;

        public ListViewModel(SectionConfigBase<T> sectionConfig, int visibleItems = 0)
            : base(sectionConfig)
        {
            _visibleItems = visibleItems;
            _items = new ObservableCollection<ItemViewModel>();

            _sectionConfig = sectionConfig;

            Title = sectionConfig.ListPage.Title;
            NavigationInfo = _sectionConfig.ListNavigationInfo;

            if (!DataProvider.IsLocal)
            {
                Actions.Add(new ActionInfo
                {
                    Command = Refresh,
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    Type = ActionType.Primary
                });
            }
        }

        public RelayCommand<ItemViewModel> ItemClickCommand
        {
            get
            {
                return new RelayCommand<ItemViewModel>(item =>
                    {
                        NavigationService.NavigateTo(item);
                    });
            }
        }

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

        public NavigationInfo NavigationInfo { get; set; }

        public ObservableCollection<ItemViewModel> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }

        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
            private set { SetProperty(ref _hasMoreItems, value); }
        }

        public ICommand Refresh
        {
            get
            {
                return new RelayCommand( async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public void ShareContent(DataRequest dataRequest, bool supportsHtml = true)
        {
            if (Items != null && Items.Count > 0)
            {
                ShareContent(dataRequest, Items[0], supportsHtml);
            }
        }

        protected override void ParseItems(CachedContent<T> content, ItemViewModel selectedItem)
        {
            var parsedItems = new List<ItemViewModel>();
            IEnumerable<T> sourceVisibleItems = null;
            if (_visibleItems == 0)
            {
                sourceVisibleItems = content.Items;
            }
            else
            {
                sourceVisibleItems = content.Items.Take(_visibleItems);
            }

            foreach (var item in sourceVisibleItems)
            {
                var parsedItem = new ItemViewModel
                    {
                        _id = item._id,
                        NavigationInfo = _sectionConfig.ListPage.NavigationInfo(item)
                    };
                _sectionConfig.ListPage.LayoutBindings(parsedItem, item);

                parsedItems.Add(parsedItem);
            }

            Items.Sync(parsedItems);
            HasMoreItems = content.Items.Count() > Items.Count; 
        }

    }
}
