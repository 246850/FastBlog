using System;
using System.Collections.Generic;
using System.Text;

namespace FastBlog.Models
{
    public class PagedList<T> : List<T> where T : class, new()
    {
        public int Page { get; set; }
        public long Total { get; set; }
        public int PageSize { get; set; }
        public long PageCount
        {
            get
            {
                if (PageSize == 0) return 0;
                var i = (Total / PageSize);
                if (Total % PageSize == 0)
                {
                    return i;
                }
                return i + 1;
            }
        }
    }
}
