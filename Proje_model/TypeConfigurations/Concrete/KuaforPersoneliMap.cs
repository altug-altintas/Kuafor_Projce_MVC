using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proje_model.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.TypeConfigurations.Concrete
{
    public class KuaforPersoneliMap : IEntityTypeConfiguration<KuaforPersoneli>
    {
        public void Configure(EntityTypeBuilder<KuaforPersoneli> builder)
        {
            builder.Property(p => p.Adi).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Soyadi).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DogumTarihi).IsRequired(false);
            builder.Property(p => p.Adres).HasMaxLength(200);
            builder.Property(p => p.Telefon).HasMaxLength(15);
            builder.Property(p => p.TC).HasMaxLength(11);
            builder.Property(p => p.IseBaslangicTarihi).IsRequired(false);
            builder.Property(p => p.Gorevi).HasMaxLength(100);
            builder.HasOne(a => a.AppUser).WithMany(a => a.kuaforPersonelis).HasForeignKey(a => a.AppUserID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
