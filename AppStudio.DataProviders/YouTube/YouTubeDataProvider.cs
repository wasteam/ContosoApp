using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Web.Http;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.InternetClient;
using AppStudio.DataProviders.YouTube.Parser;
using Newtonsoft.Json;

namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataProvider : DataProviderBase<YouTubeDataConfig, YouTubeSchema>
    {
        private const string BaseUrl = @"https://www.googleapis.com/youtube/v3";

        public YouTubeDataProvider(YouTubeDataConfig config)
            : base(config)
        {
            switch (_config.QueryType)
            {
                case "videos":
                    _parser = new YouTubeSearchParser();
                    break;
                case "channels":
                case "playlist":
                default:
                    _parser = new YouTubePlaylistParser();
                    break;
            }
        }

        public YouTubeDataProvider(YouTubeDataConfig config, IParser<YouTubeSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<YouTubeSchema>> LoadDataAsync()
        {
            IEnumerable<YouTubeSchema> result;

            switch (_config.QueryType)
            {
                case "channels":
                    result = await LoadChannelAsync();
                    break;
                case "videos":
                    result = await SearchAsync();
                    break;
                case "playlist":
                    result = await LoadPlaylistAsync(_config.Query);
                    break;
                default:
                    throw new QueryTypeNotSupportedException();
            }

            return result;
        }

        private async Task<IEnumerable<YouTubeSchema>> LoadChannelAsync()
        {
            IEnumerable<YouTubeSchema> result = new ObservableCollection<YouTubeSchema>();
            var listId = await GetUploadVideosListId();
            if (!string.IsNullOrEmpty(listId))
            {
                result = await LoadPlaylistAsync(listId);
            }
            return result;
        }

        private async Task<IEnumerable<YouTubeSchema>> SearchAsync()
        {
            InternetRequestSettings settings = new InternetRequestSettings
            {
                RequestedUri = GetSearchUrl()
            };

            var requestResult = await InternetRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                return _parser.Parse(requestResult.Result);
            }

            if (requestResult.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException();
        }

        private async Task<string> GetUploadVideosListId()
        {
            InternetRequestSettings settings = new InternetRequestSettings
            {
                RequestedUri = GetChannelUrl()
            };

            var requestResult = await InternetRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                var channel = JsonConvert.DeserializeObject<YouTubeResult<YouTubeChannelLookupResult>>(requestResult.Result);
                if (channel != null
                    && channel.items != null
                    && channel.items.Count > 0
                    && channel.items[0].contentDetails != null
                    && channel.items[0].contentDetails.relatedPlaylists != null
                    && !string.IsNullOrEmpty(channel.items[0].contentDetails.relatedPlaylists.uploads))
                {
                    return channel.items[0].contentDetails.relatedPlaylists.uploads;
                }
            }
            return string.Empty;
        }

        private async Task<IEnumerable<YouTubeSchema>> LoadPlaylistAsync(string playlistId)
        {
            InternetRequestSettings settings = new InternetRequestSettings
            {
                RequestedUri = GetPlaylistUrl(playlistId)
            };

            var requestResult = await InternetRequest.DownloadAsync(settings);
            if (requestResult.Success)
            {
                return _parser.Parse(requestResult.Result);
            }

            if (requestResult.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new OAuthKeysRevokedException();
            }

            throw new RequestFailedException();
        }

        private Uri GetChannelUrl()
        {
            return new Uri(string.Format("{0}/channels?forUsername={1}&part=contentDetails&maxResults=1&key={2}", BaseUrl, _config.Query, _config.Tokens["ApiKey"]), UriKind.Absolute);
        }

        private Uri GetPlaylistUrl(string playlistId)
        {
            return new Uri(string.Format("{0}/playlistItems?playlistId={1}&part=snippet&maxResults=20&key={2}", BaseUrl, playlistId, _config.Tokens["ApiKey"]), UriKind.Absolute);
        }

        private Uri GetSearchUrl()
        {
            return new Uri(string.Format("{0}/search?q={1}&part=snippet&maxResults=20&key={2}&type=video", BaseUrl, _config.Query, _config.Tokens["ApiKey"]), UriKind.Absolute);
        }
    }
}
