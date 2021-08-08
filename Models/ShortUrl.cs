using System;

namespace WebUrlShort.Models
{
    public class ShortUrl
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string Chunk => Id;

        public ShortUrl(Uri url)
        {
            Url = url.AbsoluteUri;
            Id = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", size: 7);
        }

        public ShortUrl()
        {

        }
    }
}
