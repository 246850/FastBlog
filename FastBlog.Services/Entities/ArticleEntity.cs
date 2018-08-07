using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Entities
{
    public class ArticleEntity:AbstractBaseEntity
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
