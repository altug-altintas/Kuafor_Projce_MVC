using System.ComponentModel.DataAnnotations;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class IslemUpdateVm
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "İşlem adı zorunludur.")]
        [MaxLength(50, ErrorMessage = "İşlem adı en fazla 50 karakter olabilir.")]
        public string IslemAdi { get; set; }

        [Required(ErrorMessage = "İşlem ücreti zorunludur.")]
        public decimal IslemUcreti { get; set; }


        public int IslemSuresi { get; set; }
    }
}
