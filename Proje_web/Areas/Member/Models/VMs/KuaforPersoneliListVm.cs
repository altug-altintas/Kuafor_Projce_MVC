using System;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class KuaforPersoneliListVm
    {
        public int ID { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Gorevi { get; set; }
    }
}
