using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.Menu;
using ContosoLtd.Config;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Sections
{
    public class ContactUsConfig : SectionConfigBase<MenuSchema>
    {
        public override DataProviderBase<MenuSchema> DataProvider
        {
            get
            {
                return new LocalStorageDataProvider<MenuSchema>(new LocalStorageDataConfig
                {
                    FilePath = "/Assets/Data/ContactUs.json"
                });
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("ContactUsListPage");
            }
        }


        public override ListPageConfig<MenuSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<MenuSchema>
                {
                    Title = "Contact us",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Title;
                        viewModel.Image = item.Icon;
                    },
                    NavigationInfo = (item) =>
                    {
                        return NavigationInfo.FromMenu(item);
                    }
                };
            }
        }

        public override DetailPageConfig<MenuSchema> DetailPage
        {
            get { return null; }
        }
    }
}
