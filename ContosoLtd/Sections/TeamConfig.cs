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
    public class TeamConfig : SectionConfigBase<Team1Schema>
    {
        public override DataProviderBase<Team1Schema> DataProvider
        {
            get
            {
                return new DynamicStorageDataProvider<Team1Schema>(new DynamicStorageDataConfig
                {
                    Url = "http://ds.winappstudio.com/api/data/collection?dataRowListId=edcbe360-47da-4ebd-9936-101ebd2b6b2f&appId=dd1bf21f-bb58-4f15-977e-4adbf1889a3b",
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
                return NavigationInfo.FromPage("TeamListPage");
            }
        }

        public override ListPageConfig<Team1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<Team1Schema>
                {
                    Title = "Team",

                    LayoutBindings = (viewModel, item) =>
                    {
                        viewModel.Title = item.Name.ToSafeString();
                        viewModel.SubTitle = item.JobTitle.ToSafeString();
                        viewModel.Description = "";
                        viewModel.Image = item.Thumbnail.ToSafeString();

                    },
                    NavigationInfo = (item) =>
                    {
                        return NavigationInfo.FromPage("TeamDetailPage", true);
                    }
                };
            }
        }

        public override DetailPageConfig<Team1Schema> DetailPage
        {
            get
            {
                var bindings = new List<Action<ItemViewModel, Team1Schema>>();

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Name.ToSafeString();
                    viewModel.Title = item.JobTitle.ToSafeString();
                    viewModel.Description = "";
                    viewModel.Image = item.Image.ToSafeString();
                    viewModel.Content = null;
                });

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Contact";
                    viewModel.Title = item.Email.ToSafeString();
                    viewModel.Description = item.Phone.ToSafeString();
                    viewModel.Image = "";
                    viewModel.Content = null;
                });

                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = "Job Description";
                    viewModel.Title = item.JobTitle.ToSafeString();
                    viewModel.Description = item.Description.ToSafeString();
                    viewModel.Image = "";
                    viewModel.Content = null;
                });

				var actions = new List<ActionConfig<Team1Schema>>
				{
                    ActionConfig<Team1Schema>.Phone("Phone", (item) => item.Phone.ToSafeString()),
                    ActionConfig<Team1Schema>.Address("OfficeLocation", (item) => item.OfficeLocation.ToSafeString()),
                    ActionConfig<Team1Schema>.Mail("Email", (item) => item.Email.ToSafeString()),
				};

                return new DetailPageConfig<Team1Schema>
                {
                    Title = "Team",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }

    }
}
