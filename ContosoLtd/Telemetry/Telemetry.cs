using Microsoft.ApplicationInsights;

namespace ContosoLtd.Telemetry
{
    public class Telemetry
    {
        private static TelemetryClient _telemetryClient;

        public static void TrackPageView(string page)
        {
            if (_telemetryClient == null)
            {
                _telemetryClient = new TelemetryClient();
            }

            _telemetryClient.TrackPageView(page);
        }

        public static void TrackException(System.Exception ex)
        {
            if (_telemetryClient == null)
            {
                _telemetryClient = new TelemetryClient();
            }

            _telemetryClient.TrackException(ex);
        }
    }
}
