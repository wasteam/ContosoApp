using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Html;
using AppStudio.DataProviders.LocalStorage;
using ContosoLtd.Config;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Sections
{
    public class AboutUsConfig : SectionConfigBase<HtmlSchema>
    {
        public override DataProviderBase<HtmlSchema> DataProvider
        {
            get
            {
                return new HtmlDataProvider(new LocalStorageDataConfig
                {
                    FilePath = "/Assets/Data/AboutUs.htm"
                });
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("AboutUsListPage");
            }
        }


        public override ListPageConfig<HtmlSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<HtmlSchema>
                {
                    Title = "About us",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Content = item.Content;
                    },
                    NavigationInfo = (item) =>
                    {
                        return null;
                    }
                };
            }
        }

        public override DetailPageConfig<HtmlSchema> DetailPage
        {
            get { return null; }
        }
    }
}
