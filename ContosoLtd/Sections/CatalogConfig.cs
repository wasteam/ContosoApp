using System;
using System.Collections.Generic;
using AppStudio.Common;
using AppStudio.Common.Actions;
using AppStudio.Common.Commands;
using AppStudio.Common.Navigation;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.DynamicStorage;
using Windows.Storage;
using ContosoLtd.Config;
using ContosoLtd.ViewModels;

namespace ContosoLtd.Sections
{
    public class CatalogConfig : SectionConfigBase<Catalog1Schema>
    {
        public override DataProviderBase<Catalog1Schema> DataProvider
        {
            get
            {
                return new DynamicStorageDataProvider<Catalog1Schema>(new DynamicStorageDataConfig
                {
                    Url = "http://ds.winappstudio.com/api/data/collection?dataRowListId=784a960e-af92-484e-ba3b-a011deaad1d6&appId=dd1bf21f-bb58-4f15-977e-4adbf1889a3b",
                    AppId = "dd1bf21f-bb58-4f15-977e-4adbf1889a3b",
                    StoreId = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.StoreIdSetting] as string,
                    DeviceType = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.DeviceTypeSetting] as string
                });
            }
        }

        public override NavigationInfo ListNavigationInfo
        {
            get 
            {
                return NavigationInfo.FromPage("CatalogListPage");
            }
        }

        public override ListPageConfig<Catalog1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Catalog1Schema>
                {
                    Title = "Catalog",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Name.ToSafeString();
                        viewModel.SubTitle = item.Reference.ToSafeString();
                        viewModel.Description = "";
                        viewModel.Image = item.Image.ToSafeString();

                    },
                    NavigationInfo = (item) =>
                    {
                        return NavigationInfo.FromPage("CatalogDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<Catalog1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, Catalog1Schema>>();

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Name.ToSafeString();
                    viewModel.Title = item.Reference.ToSafeString();
                    viewModel.Description = "";
                    viewModel.Image = item.Image.ToSafeString();
                    viewModel.Content = null;
                });

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Specs";
                    viewModel.Title = item.Specification.ToSafeString();
                    viewModel.Description = item.MoreInfo.ToSafeString();
                    viewModel.Image = "";
                    viewModel.Content = null;
                });

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Description";
                    viewModel.Title = "";
                    viewModel.Description = item.Description.ToSafeString();
                    viewModel.Image = "";
                    viewModel.Content = null;
                });

				var actions = new List<ActionConfig<Catalog1Schema>>
				{
                    ActionConfig<Catalog1Schema>.Link("MoreInfo", (item) => item.MoreInfo.ToSafeString()),
				};

                return new DetailPageConfig<Catalog1Schema>
                {
                    Title = "Catalog",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }

    }
}
