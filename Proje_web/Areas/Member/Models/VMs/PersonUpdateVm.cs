using System.ComponentModel.DataAnnotations;
using System;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class PersonUpdateVm
    {
        public int ID { get; set; } // ID alanını ekleyin

        [Required(ErrorMessage = "Adı zorunludur.")]
        [MaxLength(20, ErrorMessage = "Adı en fazla 20 karakter olmalıdır.")]
        public string Adi { get; set; }

        [Required(ErrorMessage = "Soyadı zorunludur.")]
        [MaxLength(40, ErrorMessage = "Soyadı en fazla 40 karakter olmalıdır.")]
        public string Soyadi { get; set; }

        public DateTime DogumTarihi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string TC { get; set; }
        public DateTime IseBaslangicTarihi { get; set; }
        public string Gorevi { get; set; }
    }
}
