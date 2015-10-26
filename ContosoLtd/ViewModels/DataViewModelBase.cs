using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AppStudio.Common;
using AppStudio.Common.Actions;
using AppStudio.Common.Cache;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using Windows.ApplicationModel.DataTransfer;
using ContosoLtd.Config;

namespace ContosoLtd.ViewModels
{
    public abstract class DataViewModelBase : ObservableBase
    {
        private string _title;
        private bool _hasLoadDataErrors;
        private bool _isBusy;
        private string _lastUpdated;

        public DataViewModelBase()
        {
            Actions = new List<ActionInfo>();
        }

        public string Title
        {
            get { return _title; }
            protected set { SetProperty(ref _title, value); }
        }

        public bool HasLoadDataErrors
        {
            get { return _hasLoadDataErrors; }
            protected set { SetProperty(ref _hasLoadDataErrors, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            protected set { SetProperty(ref _isBusy, value); }
        }

        public string LastUpdated
        {
            get { return _lastUpdated; }
            protected set { SetProperty(ref _lastUpdated, value); }
        }

        public List<ActionInfo> Actions { get; private set; }

        public bool HasActions
        {
            get
            {
                return Actions != null && Actions.Count > 0;
            }
        }
        
        public abstract bool HasLocalData { get; }

        public abstract Task LoadDataAsync(bool forceRefresh = false);
    }

    public abstract class DataViewModelBase<T> : DataViewModelBase where T : SchemaBase
    {
        private readonly TimeSpan CacheExpiration = new TimeSpan(2, 0, 0);

        protected abstract void ParseItems(CachedContent<T> content, ItemViewModel selectedItem);

        public DataViewModelBase(ConfigBase<T> config)
        {
            CacheKey = config.Name;
            DataProvider = config.DataProvider;
        }

        protected string CacheKey { get; private set; }

        protected DataProviderBase<T> DataProvider { get; private set; }

        public override bool HasLocalData
        {
            get { return DataProvider.IsLocal; }
        }

        public override async Task LoadDataAsync(bool forceRefresh = false)
        {
            await LoadDataAsync(null, forceRefresh);
        }

        public async Task LoadDataAsync(ItemViewModel selectedItem, bool forceRefresh = false)
        {
            try
            {
                HasLoadDataErrors = false;
                IsBusy = true;

                var dataInCache = await AppCache.GetItemsAsync<T>(CacheKey);
                if (dataInCache != null)
                {
                    LastUpdated = dataInCache.TimeStamp.ToString();
                    ParseItems(dataInCache, selectedItem);
                }

                if (CanPerformLoad() && (forceRefresh || DataNeedToBeUpdated(dataInCache, CacheExpiration)))
                {
                    var freshData = new CachedContent<T>()
                    {
                        TimeStamp = DateTime.Now,
                        Items = await DataProvider.LoadDataAsync()
                    };

                    await AppCache.AddItemsAsync(CacheKey, freshData, !DataProvider.IsLocal);

                    LastUpdated = freshData.TimeStamp.ToString();
                    ParseItems(freshData, selectedItem);
                }
            }
            catch (Exception ex)
            {
                Telemetry.Telemetry.TrackException(ex);
                HasLoadDataErrors = true;
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void ShareContent(DataRequest dataRequest, ItemViewModel item, bool supportsHtml)
        {
            if (item != null)
            {
                dataRequest.Data.Properties.Title = string.IsNullOrEmpty(item.Title) ? Title : item.Title;

                if (!string.IsNullOrEmpty(item.SubTitle))
                {
                    dataRequest.Data.SetText(Utility.DecodeHtml(item.SubTitle));
                }

                if (!string.IsNullOrEmpty(item.Description))
                {
                    SetContent(dataRequest, item.Description, supportsHtml);
                }

                if (!string.IsNullOrEmpty(item.Content))
                {
                    SetContent(dataRequest, item.Content, supportsHtml);
                }

                var imageUrl = item.Image;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    if (imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        dataRequest.Data.SetWebLink(new Uri(imageUrl));
                    }
                    else
                    {
                        imageUrl = string.Format("ms-appx://{0}", imageUrl);
                    }
                    dataRequest.Data.SetBitmap(Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(imageUrl)));
                }
            }
        }

        private void SetContent(DataRequest dataRequest, string data, bool supportsHtml)
        {
            if (supportsHtml)
            {
                dataRequest.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(data));
            }
            else
            {
                dataRequest.Data.SetText(Utility.DecodeHtml(data));
            }
        }

        private static bool DataNeedToBeUpdated(CachedContent<T> dataInCache, TimeSpan expiration)
        {
            return dataInCache == null || (DateTime.Now - dataInCache.TimeStamp > expiration);
        }

        private bool CanPerformLoad()
        {
            return DataProvider.IsLocal || NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
