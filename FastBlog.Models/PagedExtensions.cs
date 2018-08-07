using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastBlog.Models
{
    public static class PagedExtensions
    {
        public static PagedList<T> ToPaged<T>(this IEnumerable<T> source, int page, int pageSize, long total) where T : class, new()
        {
            PagedList<T> list = new PagedList<T>
            {
                Page = page,
                PageSize = pageSize,
                Total = total
            };
            list.AddRange(source);
            return list;
        }

        public static PagedList<TModel> ToPagedModel<TEntity, TModel>(this PagedList<TEntity> source, Func<TEntity, TModel> func)
            where TEntity : class, new()
            where TModel : class, new()
        {
            var list = source.Select(func);
            PagedList<TModel> result = new PagedList<TModel>
            {
                Page = source.Page,
                Total = source.Total,
                PageSize =source.PageSize
            };
            result.AddRange(list);
            return result;
        }
    }
}
