using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proje_model.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.TypeConfigurations.Concrete
{
    public class KuaforTakvimMap : IEntityTypeConfiguration<KuaforTakvim>
    {
        public void Configure(EntityTypeBuilder<KuaforTakvim> builder)
        {
            builder.Property(a => a.YapilacakisLemTarihiBaslangic).IsRequired();
            builder.Property(a => a.YapilacakisLemTarihiBitis).IsRequired();
            builder.HasOne(a => a.Islem).WithMany().HasForeignKey(a => a.Islemid).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.Personel).WithMany().HasForeignKey(t => t.Personelid).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.AppUser).WithMany(a => a.kuaforTakvims).HasForeignKey(a => a.AppUserID).OnDelete(DeleteBehavior.Restrict);


        }
    }
}
