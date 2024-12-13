using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proje_Dal.Context;
using Proje_Dal.Repositories.Interfaces.Concrete;
using Proje_model.Models.Concrete;
using Proje_model.Models.Enums;
using Proje_web.Areas.Member.Models.VMs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proje_Dal.Repositories.Concrete;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;


namespace Proje_web.Areas.Member.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Area("Member")]
    public class KuaforTakvimController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ProjectContext _project;
        private readonly IMapper _mapper;
        private readonly IIslemleRepo _islemRepo;
        private readonly IKuaforPersoneliRepo _personelRepo;
        private readonly IKuaforTakvimRepo _takvimRepo;

        public KuaforTakvimController(UserManager<AppUser> userManager, IIslemleRepo islemRepo, IKuaforPersoneliRepo personelRepo, IKuaforTakvimRepo takvimRepo, ProjectContext project, IMapper mapper)
        {
            _islemRepo = islemRepo;
            _userManager = userManager;
            _project = project;
            _mapper = mapper;
            _personelRepo = personelRepo;
            _takvimRepo = takvimRepo;
        }

        [HttpGet]
        public decimal GetIslemUcret(int id)
        {
            var islem = _islemRepo.GetDefault(a => a.ID == id);
            return islem?.isLemUcreti ?? 0;
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);

            var personeller = _personelRepo.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == userId);
            var islemler = _islemRepo.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == userId);


            var secilenIslem = islemler.FirstOrDefault(i => i.ID == 0);
            var isLemUcreti = secilenIslem != null ? secilenIslem.isLemUcreti : 0;

            var result = new
            {
                Success = true,
                User = new
                {
                    appUser.Id,
                    appUser.UserName,
                    appUser.Email
                },
                Personeller = personeller.Select(p => new { p.ID, p.Adi }),
                Islemler = islemler.Select(i => new { i.ID, i.isLemAdi }),
                isLemUcreti
            };

            return Json(result);

        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]KuaforTakvimCreateVm model)
        {
            // AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);

            if (ModelState.IsValid)
            {


                var mevcutKayit = _takvimRepo.GetDefaults(x =>
                    x.Personelid == model.Personelid &&
                    (
                        (x.YapilacakisLemTarihiBaslangic <= model.YapilacakisLemTarihiBitis && x.YapilacakisLemTarihiBaslangic >= model.YapilacakisLemTarihiBaslangic) ||
                        (x.YapilacakisLemTarihiBitis <= model.YapilacakisLemTarihiBitis && x.YapilacakisLemTarihiBitis >= model.YapilacakisLemTarihiBaslangic)
                    )
                );

                if (mevcutKayit.Any())
                {
                    ModelState.AddModelError("", "Bu personelin ranedvu kaydı mevcut. Lütfen farklı bir tarih seçin.");
                    return Json(model);
                }
                var kuaforTakvim = _mapper.Map<KuaforTakvim>(model);

                kuaforTakvim.AppUserID = userId;
                _takvimRepo.Create(kuaforTakvim);
                //return RedirectToAction("Index");
                return Json(new { success = true, redirectUrl = Url.Action("Index") });

            }

            return View(model);
        }

        public async Task<IActionResult> UpdateIslem(int id)
        {
            var takvim = await _takvimRepo.GetByIdAsync(id);
            if (takvim == null)
            {
                return NotFound();
            }
            var personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive);
            var islemler = _islemRepo.GetDefaults(x => x.Statu != Statu.Passive);

            var result = new
            {
                Success = true,
                Takvim = new
                {
                    takvim.ID,
                    takvim.YapilacakisLemTarihiBaslangic,
                    takvim.YapilacakisLemTarihiBitis,
                    takvim.Personelid,
                    takvim.Islemid
                },
                Personeller = personeller.Select(p => new { p.ID, p.Adi }),
                Islemler = islemler.Select(i => new { i.ID, i.isLemAdi })
            };

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateIslem(KuaforTakvimUpdateVm model)
        {
            //AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);

            if (ModelState.IsValid)
            {
                var mevcutKayit = _takvimRepo.GetDefaults(x =>
                    x.Personelid == model.Personelid &&
                    x.ID != model.ID &&
                    (
                        (x.YapilacakisLemTarihiBaslangic <= model.YapilacakisLemTarihiBitis && x.YapilacakisLemTarihiBaslangic >= model.YapilacakisLemTarihiBaslangic) ||
                        (x.YapilacakisLemTarihiBitis <= model.YapilacakisLemTarihiBitis && x.YapilacakisLemTarihiBitis >= model.YapilacakisLemTarihiBaslangic)
                    )
                );

                if (mevcutKayit.Any())
                {
                    ModelState.AddModelError("", "Bu personelin seçilen tarih ve saat aralığında bir kaydı mevcut. Lütfen farklı bir tarih ve saat seçin.");
                    return Json(model);
                }

                var guncellenecekTakvim = _mapper.Map<KuaforTakvim>(model);
                guncellenecekTakvim.AppUserID = userId;
                _takvimRepo.Update(guncellenecekTakvim);
                // return RedirectToAction("ListIslem");
                return Json(new { success = true, redirectUrl = Url.Action("ListIslem") });

            }

            return Json(model);
        }

        public async Task<IActionResult> ListIslem(int? selectedPersonelId, DateTime? startDate)
        {
            // AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);

            startDate = startDate ?? DateTime.Now;
            var personeller = _personelRepo.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID == userId);
            var takvimler = _takvimRepo.GetDefaults(a =>
                (!selectedPersonelId.HasValue || a.Personelid == selectedPersonelId) &&
                a.YapilacakisLemTarihiBaslangic.Date >= startDate.Value.Date && a.Statu != Statu.Passive && a.AppUserID == userId
            );
            var islemIds = takvimler.Select(a => a.Islemid).Distinct().ToList();
            var islemler = _islemRepo.GetByDefaults(a => a, a => islemIds.Contains(a.ID));

            foreach (var takvim in takvimler)
            {
                takvim.Islem = islemler.FirstOrDefault(a => a.ID == takvim.Islemid);
            }
            var model = new TakvimVm
            {
                SelectedPersonelId = selectedPersonelId ?? 0,
                Personeller = personeller,
                Takvimler = takvimler,
                SelectedPersoneAdi = selectedPersonelId.HasValue
                    ? personeller.FirstOrDefault(a => a.ID == selectedPersonelId && a.AppUserID == userId)?.Adi
                    : null,
                IslemAdi = takvimler.FirstOrDefault()?.Islem?.isLemAdi,
                StartDate = startDate
            };

            return Json(model);
        }



        [HttpGet]
        public IActionResult GetIslemSuresi(int id)
        {
            var islem = _islemRepo.GetDefaults(a => a.ID == id).FirstOrDefault();
            if (islem != null)
            {
                return Json(islem.isLemSuresi);
            }
            return Json(0);
        }
        public async Task<IActionResult> Index()
        {
            //AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);

            var personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive && x.AppUserID == userId;
            var model = new TakvimVm
            {
                Personeller = personeller,
                Takvimler = Enumerable.Empty<KuaforTakvim>(),
                SelectedPersonelId = 0,
                SelectedPersoneAdi = string.Empty
            };

            if (model.Takvimler == null)
            {
                model.Takvimler = new List<KuaforTakvim>();
            }

            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]TakvimVm model)
        {
            //AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);
            if (model.SelectedPersonelId > 0)
            {

                var takvimler = _takvimRepo.GetDefaults(a => a.Personelid == model.SelectedPersonelId && a.Statu != Statu.Passive && a.AppUserID == userId);
                var islemIds = takvimler.Select(a => a.Islemid).Distinct().ToList();
                var islemler = _islemRepo.GetByDefaults(a => a, a => islemIds.Contains(a.ID));

                foreach (var takvim in takvimler)
                {
                    takvim.Islem = islemler.FirstOrDefault(a => a.ID == takvim.Islemid);
                }

                model.Takvimler = takvimler;

                var selectedPersonel = _personelRepo.GetDefaults(a => a.ID == model.SelectedPersonelId).FirstOrDefault();
                if (selectedPersonel != null)
                {
                    model.SelectedPersoneAdi = selectedPersonel.Adi;
                }
            }
            else
            {
                ModelState.AddModelError("SelectedPersonelId", "Lütfen bir personel seçin.");

            }

            model.Personeller = _personelRepo.GetDefaults(a => a.Statu != Statu.Passive && a.AppUserID ==userId);
            return Json(model);
        }

        [HttpPost]
        public IActionResult DeleteIslem(int id)
        {
            KuaforTakvim kuaforTakvim = _takvimRepo.GetDefault(a => a.ID == id);
            _takvimRepo.Delete(kuaforTakvim);
            //return RedirectToAction("ListIslem");
            return Json(new { success = true, redirectUrl = Url.Action("ListIslem") });

        }



        [HttpGet]
        public async Task<IActionResult> GetTakvimByPersonel(int id)
        {
            // AppUser appUser = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var appUser = await _userManager.FindByIdAsync(userId);
            var takvimler = _takvimRepo.GetDefaults(a => a.Personelid == id && a.AppUserID == userId)
                                         .Select(a => new
                                         {
                                             islemAdi = _islemRepo.GetDefaults(y => y.ID == a.Islemid).FirstOrDefault().isLemAdi,
                                             yapilacakisLemTarihiBaslangic = a.YapilacakisLemTarihiBaslangic,
                                             yapilacakisLemTarihiBitis = a.YapilacakisLemTarihiBitis
                                         }).ToList();

            return Json(takvimler);
        }

    }
}


