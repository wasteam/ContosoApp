using System;
using AppStudio.Common;
using Windows.ApplicationModel;

namespace ContosoLtd.ViewModels
{
    public class AboutThisAppViewModel : ObservableBase
    {
        public string Publisher
        {
            get
            {
                return "Microsoft Corporation";
            }
        }

        public string AppVersion
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}", Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
            }
        }

        public string AboutText
        {
            get
            {
                return "Everything you need to know about a company in a quick and simple app.  Use this " +
    "template to create an app about your Company";
            }
        }
    }
}

