using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FastBlog.Models;
using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastBlog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _service;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        public ArticleController(IArticleService service,
            ICategoryService categoryService,
            IAccountService accountService)
        {
            _service = service;
            _categoryService = categoryService;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult List(PagerItem pager, int categoryId = -1)
        {
            pager.PageSize = 20;
            PagedList<ArticleEntity> entities = _service.GetPagedList(pager.Page, pager.PageSize, categoryId);
            ViewBag.CategoryId = categoryId;
            
            return View(entities);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            ArticleEntity entity = _service.Get(id);
            if (entity == null) return NotFound();
            ArticleModel model = new ArticleModel
            {
                Id = entity.Id,
                CategoryId = entity.CategoryId,
                Title = entity.Title,
                Content = entity.Content,
                CreateTime = entity.CreateTime
            };

            // 用户
            var account = _accountService.Get(entity.AccountId);
            model.AccountName = account.Name;

            // 分类
            var category = _categoryService.Get(entity.CategoryId);
            model.CategoryTitle = category.Title;

            // 上一篇/下一篇
            ArticleEntity prev = _service.GetPrevById(id);
            ArticleEntity next = _service.GetNextById(id);
            if (prev != null)
            {
                model.Prev = new ArticleModel { Id = prev.Id, Title = prev.Title, CreateTime = prev.CreateTime };
            }
            if (next != null)
            {
                model.Next = new ArticleModel { Id = next.Id, Title = next.Title, CreateTime = next.CreateTime };
            }
            return View(model);
        }
    }
}