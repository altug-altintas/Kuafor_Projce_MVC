using Proje_model.Models.Concrete;
using System.Collections.Generic;   
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Proje_web.Models.VMs
{
    public class HMTakvimVm
    {


        public int? SelectedPersonelId { get; set; }
        public string SelectedPersoneAdi { get; set; }
        public IEnumerable<KuaforPersoneli> Personeller { get; set; }
        public IEnumerable<KuaforTakvim> Takvimler { get; set; }
        public string IslemAdi { get; set; }
        public string KuaforAdi { get; set; }
        public string AppUserId { get; set; }

    }
}
