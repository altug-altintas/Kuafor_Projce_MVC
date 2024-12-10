using Proje_model.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.Models.Concrete
{
    public class Islemler :BaseEntity
    { 
        public string  isLemAdi { get; set; }

        public decimal isLemUcreti { get; set; }   

        public int isLemSuresi { get; set; }
        public string AppUserID { get; set; }   // idendtiy  küpüphanesinden olduğu için  int değil string aldık
        public AppUser AppUser { get; set; }

    }
}
