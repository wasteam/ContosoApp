using AppStudio.DataProviders;
using AppStudio.Common.Navigation;

namespace ContosoLtd.Config
{
    public abstract class SectionConfigBase<T> : ConfigBase<T> where T : SchemaBase
    {
        public abstract NavigationInfo ListNavigationInfo { get; }
        public abstract ListPageConfig<T> ListPage { get; }
        public abstract DetailPageConfig<T> DetailPage { get; }
    }
}

