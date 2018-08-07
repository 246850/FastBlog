using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models.ViewModels
{
    public class CategoryModel:AbstractBaseModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Sequence { get; set; }

        /// <summary>
        /// 文章数
        /// </summary>
        public long ArticleCount { get; set; }
    }
}
