using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FastBlog.Models;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastBlog.Web.Areas.Admin.Controllers
{
    public class ImageController : AdminController
    {
        private readonly IImageService _service;
        private readonly IImageCategoryService _categoryService;
        public ImageController(IImageService service,
            IImageCategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult List([FromQuery]PagerItem pager, int categoryId = -1)
        {
            pager.PageSize = 15;
            PagedList<ImageEntity> entities = _service.GetPagedList(pager.Page, pager.PageSize, categoryId);
            ViewBag.CategoryList = _categoryService.GetAll();
            ViewBag.CategoryId = categoryId;
            return View(entities);
        }


        [HttpPost]
        public IActionResult Upload([FromServices]IHostingEnvironment environment)
        {
            var temp = Request.Form["categoryId"];

            IFormFile file = Request.Form.Files["upload_name"];
            if (file == null) return Content("请选择上传图片");

            string extension = Path.GetExtension(file.FileName),
                fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmsss")}{extension}",
                filePath = Path.Combine(environment.WebRootPath, "uploads"),
                fullPath = Path.Combine(filePath, fileName),
                vPath = Path.Combine("\\uploads",fileName);

            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            using (var stream = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                file.CopyTo(stream);
            }

            int categoryId = Convert.ToInt32(temp.Single());
            _service.Add(new ImageEntity
            {
                CategoryId = categoryId,
                FilePath = vPath
            });
            return Redirect("/admin/image/list");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            int result = _service.Delete(id);

            return Redirect("/admin/image/list");
        }
    }
}