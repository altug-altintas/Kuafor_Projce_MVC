using System;
using System.ComponentModel.DataAnnotations;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class KuaforPersoneliCreateVm
    {

        [Required(ErrorMessage = "Adı zorunludur.")]
        [MaxLength(20, ErrorMessage = "Ad en fazla 20 karakter olmalıdır.")]
        public string Adi { get; set; }

        [Required(ErrorMessage = "Soyadı zorunludur.")]
        [MaxLength(40, ErrorMessage = "Soyad en fazla 40 karakter olmalıdır.")]
        public string Soyadi { get; set; }

        public DateTime? DogumTarihi { get; set; }

        [MaxLength(100, ErrorMessage = "Adres en fazla 100 karakter olmalıdır.")]
        public string Adres { get; set; }

        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        [MaxLength(15, ErrorMessage = "Telefon numarası en fazla 15 karakter olmalıdır.")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası zorunludur.")]
        [StringLength(11, ErrorMessage = "TC Kimlik Numarası 11 haneli olmalıdır.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "TC Kimlik Numarası yalnızca rakamlardan oluşmalıdır.")]
        public string TC { get; set; } 

        [Required(ErrorMessage = "Görevi zorunludur.")]
        [MaxLength(50, ErrorMessage = "Görev en fazla 50 karakter olmalıdır.")]
        public string Gorevi { get; set; }

        public DateTime? IseBaslangicTarihi { get; set; }
        public string AppUserID { get; set; }

    }

}

