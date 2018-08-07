using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FastBlog.Models.ViewModels;
using FastBlog.Services;
using FastBlog.Services.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FastBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountModel model)
        {
            // 判断账号/密码是否正确
            if (model == null || string.IsNullOrWhiteSpace(model.Account) || string.IsNullOrWhiteSpace(model.Pwd))
            {
                ModelState.AddModelError("login", "请求参数错误");
                return View(model);
            }
            AccountEntity user = _service.Login(new AccountEntity
            {
                Account = model.Account.Trim(),
                Pwd = model.Pwd.Trim()
            });
            if (user == null)
            {
                ModelState.AddModelError("login", "账号/密码不正确");
                return View(model);
            }

            // 写入cookie
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.Sid, user.Id.ToString()),
                 new Claim(ClaimTypes.Name, user.Name)
            }, CookieAuthenticationDefaults.AuthenticationScheme);


            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(claimsPrincipal);

            return Redirect("/admin/home/index");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/admin/account/login");
        }
    }
}