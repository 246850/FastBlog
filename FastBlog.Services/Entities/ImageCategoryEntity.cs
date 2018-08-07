using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Entities
{
    public class ImageCategoryEntity: AbstractBaseEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public int Sequence { get; set; }
    }
}
