using Proje_model.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.Models.Concrete
{
    public class KuaforPersoneli : BaseEntity
    {
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public DateTime? DogumTarihi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string TC { get; set; }
        public DateTime? IseBaslangicTarihi { get; set; }
        public string Gorevi { get; set; }
        public string AppUserID { get; set; }   // idendtiy  küpüphanesinden olduğu için  int değil string aldık
        public AppUser AppUser { get; set; }


    }
}
