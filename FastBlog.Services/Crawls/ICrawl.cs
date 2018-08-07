using FastBlog.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Crawls
{
    public interface ICrawl
    {
        string Url { get; }
        ArticleEntity Execute();
    }
}
