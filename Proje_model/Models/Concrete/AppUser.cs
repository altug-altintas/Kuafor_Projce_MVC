using Proje_model.Models.Abstract;
using Proje_model.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Proje_model.Models.Concrete
{
    public class AppUser : IdentityUser
    {


        public AppUser()
        {
           
           
            
        }

        // buraya ıd eklemyioruz çünkü ıdentityuser kütüphanesinde var 

        private DateTime _createdDate = DateTime.Now;

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        private Statu _statu = Statu.Passive;

        public Statu Statu
        {
            get { return _statu; }
            set { _statu = value; }
        }



        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Password { get; set; }

        public string ImagePath { get; set; }   // foto kaynağı

        [NotMapped]   // veritabanı atmasın diye
        public IFormFile Image { get; set; }    // foto çekip almaya, okumaya , atmaya çalışacak ama dbde olmayacak

        [NotMapped]   // notMapped olarak işaretlenen property  veritabanıo tarafında gözükmez ordaki boir kolonla eşleştirilme<
        public string Token { get; set; }

        public DateTime StatuTime { get; set; }
        //nav property

        // 1 kullanıcının çokça makalesi olabilri

        public List<Islemler> Islemlers { get; set; }
        public List<KuaforPersoneli> kuaforPersonelis { get; set; }
        public List<KuaforTakvim> kuaforTakvims { get; set; }



    }
}
