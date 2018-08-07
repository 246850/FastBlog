using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastBlog.Web.Components
{
    public class WebCategoryViewComponent:ViewComponent
    {
        private readonly ICategoryService _service;
        private readonly IArticleService _articleService;
        public WebCategoryViewComponent(ICategoryService service, IArticleService articleService)
        {
            _service = service;
            _articleService = articleService;
        }

        public IViewComponentResult Invoke()
        {
            List<CategoryEntity> entities = _service.GetAll();
            List<CategoryModel> models = entities.Select(entity =>
            {
                CategoryModel model = new CategoryModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Desc = entity.Desc,
                    Sequence = entity.Sequence,
                    CreateTime = entity.CreateTime,
                    ArticleCount = _articleService.GetCountByCategoryId(entity.Id)
                };
                return model;
            }).ToList();
            return View(models);
        }
    }
}
