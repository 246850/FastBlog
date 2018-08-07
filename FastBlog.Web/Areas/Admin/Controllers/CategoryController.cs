using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FastBlog.Web.Areas.Admin.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly ICategoryService _service;
        private readonly IArticleService _articleService;
        public CategoryController(ICategoryService service, IArticleService articleService)
        {
            _service = service;
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult List()
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

        [HttpGet]
        public IActionResult Add()
        {
            return View(new CategoryEntity());
        }

        [HttpPost]
        public IActionResult Add(CategoryEntity model)
        {
            int result = _service.Add(model);

            return Redirect("/admin/category/list");
        }

        [HttpGet]
        public IActionResult Modify(int id)
        {
            var model = _service.Get(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Modify(CategoryEntity model)
        {
            int result = _service.Update(model);

            return Redirect($"/admin/category/modify/{model.Id}");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // 判断是否存在此分类的 文章
            bool flag = _articleService.ExistByCategoryId(id);
            // 分类下不存在 文章 才删除
            if (!flag) { int result = _service.Delete(id); }

            return Redirect("/admin/category/list");
        }
    }
}