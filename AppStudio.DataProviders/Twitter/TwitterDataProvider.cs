using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AppStudio.DataProviders.Exceptions;
using AppStudio.DataProviders.Twitter.Parser;

namespace AppStudio.DataProviders.Twitter
{
    public class TwitterDataProvider : DataProviderBase<TwitterDataConfig, TwitterSchema>
    {
        public TwitterDataProvider(TwitterDataConfig config)
            : base(config)
        {
            switch (_config.QueryType.ToLower())
            {
                case "search":
                    _parser = new TwitterSearchParser();
                    break;
                case "hometimeline":
                case "usertimeline":
                default:
                    _parser = new TwitterTimelineParser();
                    break;
            }
        }

        public TwitterDataProvider(TwitterDataConfig config, IParser<TwitterSchema> parser)
            : base(config, parser)
        {
        }

        public override async Task<IEnumerable<TwitterSchema>> LoadDataAsync()
        {
            IEnumerable<TwitterSchema> items;
            switch (_config.QueryType.ToLower())
            {
                case "hometimeline":
                    items = await this.GetHomeTimeLineAsync();
                    break;
                case "usertimeline":
                    items = await this.GetUserTimeLineAsync(_config.Query);
                    break;
                case "search":
                    items = await this.SearchAsync(_config.Query);
                    break;
                default:
                    throw new QueryTypeNotSupportedException();
            }

            return items;
        }

        private async Task<IEnumerable<TwitterSchema>> GetUserTimeLineAsync(string screenName)
        {
            try
            {
                var uri = new Uri(string.Format("https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={0}", screenName));

                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _config.Tokens);

                return _parser.Parse(rawResult);
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new UserNotFoundException(screenName);
                    }
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }

        private async Task<IEnumerable<TwitterSchema>> GetHomeTimeLineAsync()
        {
            try
            {
                var uri = new Uri("https://api.twitter.com/1.1/statuses/home_timeline.json");

                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _config.Tokens);

                return _parser.Parse(rawResult);
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }

        private async Task<IEnumerable<TwitterSchema>> SearchAsync(string hashTag)
        {
            try
            {
                var uri = new Uri(string.Format("https://api.twitter.com/1.1/search/tweets.json?q={0}", Uri.EscapeDataString(hashTag)));
                OAuthRequest request = new OAuthRequest();
                var rawResult = await request.ExecuteAsync(uri, _config.Tokens);

                return _parser.Parse(rawResult);
            }
            catch (WebException wex)
            {
                HttpWebResponse response = wex.Response as HttpWebResponse;
                if (response != null)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        throw new TooManyRequestsException();
                    }
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new OAuthKeysRevokedException();
                    }
                }
                throw;
            }
        }
    }
}
