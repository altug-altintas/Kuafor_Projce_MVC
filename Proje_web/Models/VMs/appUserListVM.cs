using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Proje_web.Models.VMs
{
    public class appUserListVM
    {

        public List<SelectListItem> AppUsers { get; set; }

        // Seçilen AppUserId
        public int SelectedAppUserId { get; set; }

        // Seçilen AppUser'ın Adı
        public string SelectedAppUserName { get; set; }
    }
}
