﻿using Proje_web.Areas.Admin.Models.VMs;
using System.Collections.Generic;

namespace Proje_web.Areas.Member.Models.VMs
{
    public class UlkeDto
    {
        public string UlkeKodu { get; set; }
        public string UlkeAdi { get; set; }
       
        public ICollection<SehirDto> Sehirler { get; set; }
    }
}
