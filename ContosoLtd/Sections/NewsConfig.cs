using System;
using System.Collections.Generic;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Bing;
using ContosoLtd.Config;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Sections
{
    public class NewsConfig : SectionConfigBase<BingSchema>
    {
        public override DataProviderBase<BingSchema> DataProvider
        {
            get
            {
                return new BingDataProvider(new BingDataConfig
                {
                    QueryType = "us", 
                    Query = @"Contoso"
                });
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("NewsListPage");
            }
        }


        public override ListPageConfig<BingSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<BingSchema>
                {
                    Title = "News",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title.ToSafeString();
                        viewModel.SubTitle = item.Summary.ToSafeString();
                        viewModel.Description = null;
                        viewModel.Image = null;

                    },
                    NavigationInfo = (item) =>
                    {
                        return new NavigationInfo
                        {
                            Type = NavigationType.DeepLink,
                            TargetUri = new Uri(item.Link)
                        };
                    }
                };
            }
        }

        public override DetailPageConfig<BingSchema> DetailPage
        {
            get { return null; }
        }
    }
}
