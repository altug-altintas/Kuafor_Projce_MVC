﻿using Proje_model.Models.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Proje_web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]

    public class AppAdminController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public AppAdminController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> LogOut()
        {

            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("JwtToken");

            return Redirect("~/");   // redirectionAction("index","home"); yerine 

        }
    }
}
