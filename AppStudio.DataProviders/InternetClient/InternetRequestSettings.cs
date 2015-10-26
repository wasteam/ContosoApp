using System;
using System.Net;

namespace AppStudio.DataProviders.InternetClient
{
    internal class InternetRequestSettings
    {
        public InternetRequestSettings()
        {
            this.Headers = new WebHeaderCollection();
        }

        public Uri RequestedUri { get; set; }

        public string UserAgent { get; set; }

        public WebHeaderCollection Headers { get; private set; }
    }
}
