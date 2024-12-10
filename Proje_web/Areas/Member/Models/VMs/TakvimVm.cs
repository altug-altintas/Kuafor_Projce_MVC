using Proje_model.Models.Concrete;
using System;
using System.Collections.Generic;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class TakvimVm
    {
        public int? SelectedPersonelId { get; set; }
        public string SelectedPersoneAdi { get; set; }
        public IEnumerable<KuaforPersoneli> Personeller { get; set; }
        public IEnumerable<KuaforTakvim> Takvimler { get; set; }
        public string IslemAdi { get; set; }
        public decimal? isLemUcreti { get; set; }

        public DateTime? StartDate { get; set; } 

    }
}
