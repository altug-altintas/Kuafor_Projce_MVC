using AutoMapper;
using Proje_web.Areas.Admin.Models.VMs;
using Proje_web.Areas.Member.Models.DTOs;
using Proje_web.Areas.Member.Models.VMs;
using Proje_web.Models.DTOs;
using Proje_model.Models.Concrete;
using Proje_web.Models.VMs;

namespace Proje_web.Models.AutoMappers
{
    public class Mappers : Profile
    {

        public Mappers()
        {

            // maplemeler
            CreateMap<RegisterDTO, AppUser>();    // registerDTO --- > appUser nesnesi teslim etmeslisin


            CreateMap<UserUpdateDTO, AppUser>().ReverseMap();

            CreateMap<Areas.Admin.Models.VMs.UlkeDto, Ulke>().ReverseMap();
            CreateMap<Areas.Member.Models.VMs.UlkeDto, Ulke>().ReverseMap();
            CreateMap<Areas.Member.Models.VMs.KuaforPersoneliCreateVm, KuaforPersoneli>().ReverseMap();
            CreateMap<PersonUpdateVm, KuaforPersoneli>().ReverseMap(); ;
            CreateMap<KuaforPersoneliListVm, KuaforPersoneli>().ReverseMap(); ;


            CreateMap<Areas.Member.Models.VMs.IslemCreateVm, Islemler>().ReverseMap();
            CreateMap<Areas.Member.Models.VMs.IslemUpdateVm, Islemler>().ReverseMap();            
            CreateMap<IslemListVm, Islemler>().ReverseMap();
            CreateMap<HMTakvimVm, KuaforTakvim>().ReverseMap();
            CreateMap<appUserListVM, AppUser>().ReverseMap();
            CreateMap<KuaforTakvimCreateVm, KuaforTakvim>().ReverseMap();
            CreateMap<KuaforTakvimUpdateVm, KuaforTakvim>().ReverseMap();
            




        }
    }
}
