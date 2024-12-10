using Proje_model.Models.Concrete;
using Proje_model.TypeConfigurations.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_Dal.Context
{
    public class ProjectContext : IdentityDbContext<AppUser>

    {

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }       
        public DbSet<OldPasswordHistory> oldPasswordHistories { get; set; }     
        public DbSet<Ulke> Ulkeler { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }

        public DbSet<Islemler> Islemler { get; set; }
        public DbSet<KuaforPersoneli> KuaforPersoneli { get; set; }
        public DbSet<KuaforTakvim> KuaforTakvim { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppUserMap());            
            modelBuilder.ApplyConfiguration(new IdentityRoleMap());
            modelBuilder.ApplyConfiguration(new UlkeMap());
            modelBuilder.ApplyConfiguration(new SehirMap());
            modelBuilder.ApplyConfiguration(new IslemlerMap());
            modelBuilder.ApplyConfiguration(new KuaforTakvimMap());
            modelBuilder.ApplyConfiguration(new KuaforPersoneliMap());

            base.OnModelCreating(modelBuilder);

        }
    }
}
