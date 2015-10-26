using System;

namespace AppStudio.DataProviders.Instagram.Parser
{
    internal class InstagramResponseItem
    {
        private readonly DateTime UnixEpochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public string id { get; set; }
        public string link { get; set; }
        public int created_time { get; set; }
        public InstagramUser user { get; set; }
        public InstagramImages images { get; set; }
        public InstagramCaption caption { get; set; }

        public InstagramSchema ToSchema()
        {
            var result = new InstagramSchema();

            result._id = this.id;
            result.SourceUrl = this.link;
            result.Published = UnixEpochDate.AddSeconds(this.created_time);
            if (this.user != null)
            {
                result.Author = this.user.username;
            }
            result.ThumbnailUrl = this.images.thumbnail.url;
            result.ImageUrl = this.images.standard_resolution.url;
            if (this.caption != null)
            {
                result.Title = this.caption.text;
            }

            return result;
        }
    }
}
