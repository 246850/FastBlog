using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Entities
{
    public class ImageEntity : AbstractBaseEntity
    {
        public int CategoryId { get; set; }
        public string FilePath { get; set; }
    }
}
