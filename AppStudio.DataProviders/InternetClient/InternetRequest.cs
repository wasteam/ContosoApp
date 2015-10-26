using System;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AppStudio.DataProviders.InternetClient
{
    internal static class InternetRequest
    {
        internal static async Task<InternetRequestResult> DownloadAsync(InternetRequestSettings settings)
        {
            InternetRequestResult result = new InternetRequestResult();
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                if (!string.IsNullOrEmpty(settings.UserAgent))
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(settings.UserAgent);
                }

                HttpResponseMessage response = await httpClient.GetAsync(settings.RequestedUri);
                result.StatusCode = response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    FixInvalidCharset(response);
                    result.Result = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Result = string.Empty;
            }
            return result;
        }

        private static void FixInvalidCharset(HttpResponseMessage response)
        {
            try
            {
                string charset = response.Content.Headers.ContentType.CharSet;
                if (charset.Contains("\""))
                {
                    response.Content.Headers.ContentType.CharSet = charset.Replace("\"", string.Empty);
                }
            }
            catch
            {
            }
        }
    }
}
