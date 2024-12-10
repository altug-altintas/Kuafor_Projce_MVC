using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje_Dal.Context;
using Proje_Dal.Repositories.Interfaces.Abstract;
using Proje_Dal.Repositories.Interfaces.Concrete;
using Proje_model.Models.Concrete;
using Proje_model.Models.Enums;
using Proje_web.Areas.Member.Models.VMs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proje_web.Areas.Member.Controllers
{
    [Authorize]
    [Area("Member")]
    public class KuaforPersoneliController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ProjectContext _project;
        private readonly IMapper _mapper;
        private readonly IBaseRepo<Ulke> _ulkeRepository;
        private readonly IBaseRepo<Sehir> _sehirRepository;
        private readonly IKuaforPersoneliRepo _kuaforPersoneliRepo;
        private readonly IIslemleRepo _islemleRepo;


        public KuaforPersoneliController(UserManager<AppUser> userManager, IIslemleRepo ıslemleRepo, IKuaforPersoneliRepo kuaforPersoneliRepo, ProjectContext project, IMapper mapper, IBaseRepo<Ulke> ulkeRepository, IBaseRepo<Sehir> sehirRepository)
        {
            _userManager = userManager;
            _kuaforPersoneliRepo = kuaforPersoneliRepo;
            _islemleRepo = ıslemleRepo;
            _project = project;
            _mapper = mapper;
            _ulkeRepository = ulkeRepository;
            _sehirRepository = sehirRepository;
        }

       

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CreatePerson()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(KuaforPersoneliCreateVm vmS)
        {
            AppUser appUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var personel = _mapper.Map<KuaforPersoneli>(vmS);
                personel.AppUserID = appUser.Id;
                _kuaforPersoneliRepo.Create(personel);
                return RedirectToAction("ListPersonel");
            }

            return View(vmS);
        }



        public async Task<IActionResult> UpdatePerson(int id)
        {
            KuaforPersoneli personeli = _kuaforPersoneliRepo.GetDefault(a => a.ID == id);
            PersonUpdateVm vm = _mapper.Map<PersonUpdateVm>(personeli);
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdatePerson(PersonUpdateVm vmS)
        {

            if (ModelState.IsValid)
            {
                var person = _mapper.Map<KuaforPersoneli>(vmS);
                _kuaforPersoneliRepo.Update(person);
                return RedirectToAction("ListPersonel");
            }

            return View(vmS);
        }


        public async Task<IActionResult> ListPersonel()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);

            var personelList = _kuaforPersoneliRepo.GetDefaults(a => a.Statu != Statu.Passive && a.Statu != Statu.Passive && a.AppUserID==appUser.Id);
            var vmList = _mapper.Map<List<KuaforPersoneliListVm>>(personelList);
            return View(vmList);
        }


        [HttpPost]
        public IActionResult DeletePersonel(int id)
        {
            KuaforPersoneli  kuaforPersoneli = _kuaforPersoneliRepo.GetDefault(a => a.ID == id);
            _kuaforPersoneliRepo.Delete(kuaforPersoneli);
            return RedirectToAction("ListPersonel");
        }







        public async Task<IActionResult> CreateIslem()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIslem(IslemCreateVm vmS)
        {
            AppUser appUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                var islem = _mapper.Map<Islemler>(vmS);
                islem.AppUserID = appUser.Id;

                _islemleRepo.Create(islem);
                return RedirectToAction("ListIslem");
            }

            return View(vmS);
        }

        public IActionResult UpdateIslem(int id)
        {
            Islemler islem = _islemleRepo.GetDefault(a => a.ID == id);
            IslemUpdateVm vm = _mapper.Map<IslemUpdateVm>(islem);
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateIslem(IslemUpdateVm vmS)
        {
            if (ModelState.IsValid)
            {
                var updateislem = _mapper.Map<Islemler>(vmS);
                _islemleRepo.Update(updateislem);

                return RedirectToAction("ListIslem");
            }

            return View(vmS);
        }


        public async Task<IActionResult> ListIslem()
        {
            AppUser appUser = await _userManager.GetUserAsync(User);

            List<Islemler> islemler = _islemleRepo.GetDefaults(a => a.Statu != Statu.Passive && a.Statu != Statu.Passive && a.AppUserID == appUser.Id);
            var vmList = _mapper.Map<List<IslemListVm>>(islemler);
            return View(vmList);
        }


        public IActionResult DeleteIslem(int id)
        {
            Islemler islemler = _islemleRepo.GetDefault(a => a.ID == id);
            _islemleRepo.Delete(islemler);
            return RedirectToAction("ListIslem");
        }



    }

}

