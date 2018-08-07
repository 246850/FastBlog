using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models.ViewModels
{
    public class ImageCategoryModel : AbstractBaseModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Sequence { get; set; }

        /// <summary>
        /// 图片数
        /// </summary>
        public long ImageCount { get; set; }
    }
}
