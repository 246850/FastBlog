using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Services.Entities
{
    public class AccountEntity:AbstractBaseEntity
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
    }
}
