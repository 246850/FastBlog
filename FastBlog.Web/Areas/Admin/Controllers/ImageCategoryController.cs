using System.Collections.Generic;
using System.Linq;
using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FastBlog.Web.Areas.Admin.Controllers
{
    public class ImageCategoryController : AdminController
    {
        private readonly IImageCategoryService _service;
        private readonly IImageService _imageService;
        public ImageCategoryController(IImageCategoryService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult List()
        {
            List<ImageCategoryEntity> entities = _service.GetAll();
            List<ImageCategoryModel> models = entities.Select(entity =>
            {
                ImageCategoryModel model = new ImageCategoryModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Desc = entity.Desc,
                    Sequence = entity.Sequence,
                    CreateTime = entity.CreateTime,
                    ImageCount = _imageService.GetCountByCategoryId(entity.Id)
                };
                return model;
            }).ToList();
            return View(models);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new ImageCategoryEntity());
        }

        [HttpPost]
        public IActionResult Add(ImageCategoryEntity model)
        {
            int result = _service.Add(model);

            return Redirect("/admin/imagecategory/list");
        }

        [HttpGet]
        public IActionResult Modify(int id)
        {
            var model = _service.Get(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Modify(ImageCategoryEntity model)
        {
            int result = _service.Update(model);

            return Redirect($"/admin/imagecategory/modify/{model.Id}");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // 判断是否存在此分类的 文章
            bool flag = _imageService.ExistByCategoryId(id);
            // 分类下不存在 文章 才删除
            if (!flag) { int result = _service.Delete(id); }

            return Redirect("/admin/imagecategory/list");
        }
    }
}