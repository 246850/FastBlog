using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models.ViewModels
{
    public class AccountModel: AbstractBaseModel
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
    }
}
