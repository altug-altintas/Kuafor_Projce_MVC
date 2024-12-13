using AutoMapper;
using Proje_web.Models.DTOs;
using Proje_Dal.Repositories.Interfaces.Concrete;
using Proje_model.Models.Concrete;
using Proje_model.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Proje_Dal.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Proje_web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAppUserRepo _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(IMapper mapper, IAppUserRepo userRepo, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public IActionResult Register()
        {
            return View();
        }



        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dTO)
        {
            if (ModelState.IsValid)   //automapper var unutma 
            {
                bool isEmailUnique = _userRepo.IsEmailUnique(dTO.Email);
                bool isUserNameUnique = _userRepo.IsUserlUnique(dTO.UserName);

                if (isEmailUnique && isUserNameUnique)
                {
                    AppUser appUser = _mapper.Map<AppUser>(dTO);

                    var image = Image.Load(dTO.Image.OpenReadStream());
                    image.Mutate(a => a.Resize(70, 70));
                    image.Save($"wwwroot/Resimler/{appUser.UserName}.jpg");
                    appUser.ImagePath = $"/Resimler/{appUser.UserName}.jpg";

                    await _userRepo.Create(appUser);
                    return Json(new { success = true, redirectUrl = Url.Action("Login") });
                }

                if (!isEmailUnique)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                }

                if (!isUserNameUnique)
                {
                    ModelState.AddModelError("UserName", "Bu kullanıcı adı zaten kullanılıyor.");
                }



            }


            return Json(dTO);
        }
        //  deneme   
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)  //kişinin ulaşmak istediği sayfa 
        {

            return Json(new LoginDTO() { ReturnUrl = returnUrl });
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dTO)
        {

            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(dTO.Email);
                var user = _userRepo.Authentication(dTO.Email, dTO.Password);

                if (appUser != null)
                {
                    if (appUser.StatuTime <= DateTime.Now)
                    {
                        appUser.Statu = Statu.Passive;
                        await _userManager.UpdateAsync(appUser);
                    }
                    if (appUser.StatuTime >= DateTime.Now)
                    {
                        appUser.Statu = Statu.Active;
                        await _userManager.UpdateAsync(appUser);
                    }

                    if (appUser.Statu == Statu.Active || appUser.Statu == Statu.Modified)
                    {
                        SignInResult result = await _signInManager.PasswordSignInAsync(appUser.UserName, dTO.Password, false, false);

                        if (result.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(appUser);
                            //   var token = GenerateJwtToken(appUser);

                            if (roles.Contains("Admın"))
                            {
                                string redirectUrl = "/admin/AppAdmin/index";
                                return Json(new { success = true, token = user.Token, redirectUrl = redirectUrl });
                            }
                            else
                            {
                                string redirectUrl = "/member/appuser/index";
                                return Json(new { success = true, token = user.Token, redirectUrl = redirectUrl });
                            }

                        }
                        else
                        {
                            return Json(new { success = false, message = "Giriş başarısız oldu" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Kullanıcı aktif değil" });
                    }



                }
                else
                {
                    return Json(new { success = false, message = "Kullanıcı Mevcut değil" });
                }

            }
            return Json(dTO);

        }
    }
}
