using System;
using System.Reflection;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ContosoLtd.Telemetry
{
    public class TelemetryInitializer : IContextInitializer
    {
        private string _operatingSystem;
        private string _sessionGuid;
        private string _appVersion;

        public TelemetryInitializer(string operatingSystem)
        {
            _operatingSystem = operatingSystem;
            _sessionGuid = Guid.NewGuid().ToString();
            _appVersion = typeof(TelemetryInitializer).GetTypeInfo().Assembly.GetName().Version.ToString();
        }

        public void Initialize(TelemetryContext context)
        {
            context.Session.Id = _sessionGuid;
            context.Component.Version = _appVersion;
            context.Device.OperatingSystem = _operatingSystem;
        }
    }
}
