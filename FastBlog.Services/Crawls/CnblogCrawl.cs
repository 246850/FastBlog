using System;
using System.Net.Http;
using System.Text;
using FastBlog.Services.Entities;
using HtmlAgilityPack;

namespace FastBlog.Services.Crawls
{
    public class CnblogCrawl : ICrawl
    {
        public CnblogCrawl(string url)
        {
            Url = url;
        }
        public string Url { get; }

        public ArticleEntity Execute()
        {
            using (HttpClient client = new HttpClient())
            {
                string body = client.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
                HtmlDocument html = new HtmlDocument();
                html.LoadHtml(body);
                HtmlNode document = html.DocumentNode;
                HtmlNode titleNode = document.SelectSingleNode("//*[@id='cb_post_title_url']"),
                    contentNode = document.SelectSingleNode("//*[@id='cnblogs_post_body']");
                if (titleNode == null || contentNode == null) return new ArticleEntity();

                return new ArticleEntity
                {
                    Title = titleNode.InnerText,
                    Content = contentNode.InnerHtml
                };
            }
        }
    }
}
