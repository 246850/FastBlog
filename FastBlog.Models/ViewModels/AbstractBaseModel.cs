using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models.ViewModels
{
    public abstract class AbstractBaseModel
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
