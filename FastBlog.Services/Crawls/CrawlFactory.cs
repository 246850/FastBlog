using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Crawls
{
    public class CrawlFactory
    {
        public static ICrawl Create(string url)
        {
            Uri uri = new Uri(url);
            ICrawl crawl = null;
            switch (uri.Host)
            {
                case "www.cnblogs.com":
                case "cnblogs.com":
                    crawl = new CnblogCrawl(url);
                    break;
                default:
                    crawl = new CnblogCrawl(url);
                    break;
            }

            return crawl;
        }
    }
}
