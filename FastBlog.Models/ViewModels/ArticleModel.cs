using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models.ViewModels
{
    public class ArticleModel:AbstractBaseModel
    {
        public string AccountName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public ArticleModel Prev { get; set; }
        public ArticleModel Next { get; set; }
    }
}
