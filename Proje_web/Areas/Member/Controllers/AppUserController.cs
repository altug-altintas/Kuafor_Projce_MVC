using Proje_model.Models.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Proje_web.Areas.Member.Models.VMs;
using System.Linq;
using AutoMapper;
using Proje_Dal.Context;
using Proje_Dal.Repositories.Interfaces.Concrete;
using Proje_model.Models.Enums;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Proje_web.Areas.Member.Controllers
{
    [Authorize]
    [Area("Member")]
    public class AppUserController : Controller
    {
        


        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ProjectContext _project;
        private readonly IMapper _mapper;
        private readonly IIslemleRepo _islemRepo;
        private readonly IKuaforPersoneliRepo _personelRepo;
        private readonly IKuaforTakvimRepo _takvimRepo;

        public AppUserController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager, IIslemleRepo islemRepo, IKuaforPersoneliRepo personelRepo, IKuaforTakvimRepo takvimRepo, ProjectContext project, IMapper mapper)
        {
            _signInManager = signInManager;
            _islemRepo = islemRepo;
            _userManager = userManager;
            _project = project;
            _mapper = mapper;
            _personelRepo = personelRepo;
            _takvimRepo = takvimRepo;
        }



        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);

            var personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive && x.AppUserID==appUser.Id);
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TakvimVm model)
        {
            AppUser appUser = await _userManager.GetUserAsync(User);
            if (model.SelectedPersonelId > 0)
            {

                var takvimler = _takvimRepo.GetDefaults(x => x.Personelid == model.SelectedPersonelId && x.Statu != Statu.Passive && x.AppUserID == appUser.Id);
                var islemIds = takvimler.Select(t => t.Islemid).Distinct().ToList();
                var islemler = _islemRepo.GetByDefaults(i => i, i => islemIds.Contains(i.ID));

                foreach (var takvim in takvimler)
                {
                    takvim.Islem = islemler.FirstOrDefault(i => i.ID == takvim.Islemid);
                }

                model.Takvimler = takvimler;

                var selectedPersonel = _personelRepo.GetDefaults(x => x.ID == model.SelectedPersonelId).FirstOrDefault();
                if (selectedPersonel != null)
                {
                    model.SelectedPersoneAdi = selectedPersonel.Adi;
                }
            }
            else
            {
                ModelState.AddModelError("SelectedPersonelId", "Lütfen bir personel seçin.");

            }

            model.Personeller = _personelRepo.GetDefaults(x => x.Statu != Statu.Passive && x.AppUserID == appUser.Id);
            return View(model);
        }



        [HttpGet]
        public async Task< IActionResult> GetEvents()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);
            var takvimler = _takvimRepo.GetDefaults(x => x.Personelid == 1 && x.AppUserID == appUser.Id);  

            var events = takvimler.Select(t => new
            {
                title = t.Islem?.isLemAdi ?? "No Appointment", 
                start = t.YapilacakisLemTarihiBaslangic.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = t.YapilacakisLemTarihiBitis.ToString("yyyy-MM-ddTHH:mm:ss")
            }).ToList();

            return Json(events);
        }



        public async Task<IActionResult> LogOut()
        {

            await _signInManager.SignOutAsync();

            return Redirect("~/");   // redirectionAction("index","home"); yerine 

        }
    }
}
