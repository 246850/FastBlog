using FastBlog.Models;
using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Crawls;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace FastBlog.Web.Areas.Admin.Controllers
{
    public class ArticleController : AdminController
    {
        private readonly IArticleService _service;
        private readonly ICategoryService _categoryService;
        public ArticleController(IArticleService service,
            ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult List(PagerItem pager, int categoryId = -1)
        {
            PagedList<ArticleEntity> entities = _service.GetPagedList(pager.Page, pager.PageSize, categoryId);
            PagedList<ArticleModel> models = entities.ToPagedModel(entity =>
            {
                ArticleModel model = new ArticleModel
                {
                    Id = entity.Id,
                    CategoryId = entity.CategoryId,
                    Title = entity.Title,
                    Content = entity.Content,
                    CreateTime = entity.CreateTime
                };

                var category = _categoryService.Get(entity.CategoryId);
                model.CategoryTitle = category.Title;
                return model;
            });

            ViewBag.CategoryId = categoryId;
            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new ArticleEntity());
        }

        [HttpPost]
        public IActionResult Add(ArticleEntity entity)
        {
            entity.AccountId = Convert.ToInt32(User.FindFirst(x => x.Type == ClaimTypes.Sid).Value);
            int result = _service.Add(entity);
            return Redirect("/admin/article/list");
        }

        [HttpGet]
        public IActionResult Modify(int id)
        {
            return View(_service.Get(id));
        }

        [HttpPost]
        public IActionResult Modify(ArticleEntity entity)
        {
            int result = _service.Update(entity);
            return Redirect($"/admin/article/modify/{entity.Id}");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            int result = _service.Delete(id);
            return Redirect("/admin/article/list");
        }

        [HttpGet]
        public IActionResult Crawl([FromQuery]string url)
        {
            ICrawl crawl = CrawlFactory.Create(url);
            ArticleEntity entity = crawl.Execute();
            return Json(entity);
        }
    }
}