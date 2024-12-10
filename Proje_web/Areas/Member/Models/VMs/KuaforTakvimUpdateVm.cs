using Proje_model.Models.Concrete;
using System.ComponentModel.DataAnnotations;
using System;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class KuaforTakvimUpdateVm
    {

        public int ID { get; set; } 


        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime YapilacakisLemTarihiBaslangic { get; set; }
        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime YapilacakisLemTarihiBitis { get; set; }

        [Required(ErrorMessage = "Ücret Zorunludur.")]
        public decimal? isLemUcreti { get; set; }

        public int Islemid { get; set; }
        public int Personelid { get; set; }

        public Islemler Islem { get; set; }
        public KuaforPersoneli Personel { get; set; }
        public string AppUserID { get; set; }   // idendtiy  küpüphanesinden olduğu için  int değil string aldık
        public AppUser AppUser { get; set; }
    }
}
