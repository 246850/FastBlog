using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Entities
{
    public abstract class AbstractBaseEntity
    {
        public AbstractBaseEntity()
        {
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
