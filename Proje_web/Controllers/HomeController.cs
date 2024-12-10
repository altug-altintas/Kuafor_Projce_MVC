using Proje_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Proje_model.Models.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Proje_Dal.Context;
using Proje_Dal.Repositories.Interfaces.Concrete;
using Proje_model.Models.Enums;
using Proje_web.Models.VMs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proje_Dal.Repositories.Concrete;
using Microsoft.AspNetCore.Authorization;

namespace Proje_web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ProjectContext _project;
        private readonly IMapper _mapper;
        private readonly IIslemleRepo _islemRepo;
        private readonly IKuaforPersoneliRepo _personelRepo;
        private readonly IKuaforTakvimRepo _takvimRepo;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, IIslemleRepo islemRepo, IKuaforPersoneliRepo personelRepo, IKuaforTakvimRepo takvimRepo, ProjectContext project, IMapper mapper)
        {
            _logger = logger;
            _islemRepo = islemRepo;
            _userManager = userManager;
            _project = project;
            _mapper = mapper;
            _personelRepo = personelRepo;
            _takvimRepo = takvimRepo;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appUsers = _userManager.Users.Where(x => x.Statu != Statu.Passive).ToList();  // Aktif kullanıcıları alıyoruz
            var model = new appUserListVM
            {
                // AppUser'ları SelectListItem olarak listeleyeceğiz
                AppUsers = appUsers.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FirstName  // Adını listeleyeceğiz
                }).ToList(),

                SelectedAppUserId = 0,
                SelectedAppUserName = string.Empty
            };

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(appUserListVM model)
        {
            

            var appUsers = _userManager.Users.Where(x => x.Statu != Statu.Passive).ToList();
            model.AppUsers = appUsers.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName
            }).ToList();

            return View(model);
        }


        [AllowAnonymous]
        [HttpGet("Kuafor/{userId}")]
        public IActionResult Kuafor(string userId)
        {
            ViewData["BodyClass"] = "no-navbar";
           
            var selectedUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            var personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive && x.AppUserID==userId);
            var model = new HMTakvimVm
            {
                Personeller = personeller,
                Takvimler = Enumerable.Empty<KuaforTakvim>(),
                SelectedPersonelId = 0,
                SelectedPersoneAdi = string.Empty,
                KuaforAdi = selectedUser.FirstName,
                AppUserId=userId


            };

            if (model.Takvimler == null)
            {
                model.Takvimler = new List<KuaforTakvim>();
            }
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost("Kuafor/{userId}")]
        public async Task<IActionResult> Kuafor(HMTakvimVm model)
        {

            var appUserId = model.AppUserId;
           
            if (model.SelectedPersonelId > 0)
            {
                var selectedUser = _userManager.Users.FirstOrDefault(x => x.Id == appUserId);

                var takvimler = _takvimRepo.GetDefaults(x => x.Personelid == model.SelectedPersonelId && x.Statu != Statu.Passive );
                var islemIds = takvimler.Select(t => t.Islemid).Distinct().ToList();
                var islemler = _islemRepo.GetByDefaults(i => i, i => islemIds.Contains(i.ID));

                foreach (var takvim in takvimler)
                {
                    takvim.Islem = islemler.FirstOrDefault(i => i.ID == takvim.Islemid);
                }
                model.Takvimler = takvimler;
                var selectedPersonel = _personelRepo.GetDefaults(x => x.ID == model.SelectedPersonelId && x.AppUserID == appUserId).FirstOrDefault();
                if (selectedPersonel != null)
                {
                    model.SelectedPersoneAdi = selectedPersonel.Adi;
                    model.KuaforAdi = selectedUser.FirstName;
                    
                }
            }
            else
            {
                ModelState.AddModelError("SelectedPersonelId", "Lütfen bir personel seçin.");

            }

            model.Personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive && x.AppUserID == appUserId);
            return View(model);
        }


        private List<KuaforTakvim> GetTakvimForMonth(int personelId, DateTime startOfMonth)
        {
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            return _project.KuaforTakvim
                           .Where(t => t.Personelid == personelId &&
                                       t.YapilacakisLemTarihiBaslangic >= startOfMonth &&
                                       t.YapilacakisLemTarihiBitis <= endOfMonth)
                           .ToList();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new  Proje_web.Models.ErrorViewModel{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
