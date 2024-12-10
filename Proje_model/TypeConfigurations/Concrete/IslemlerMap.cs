using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proje_model.Models.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.TypeConfigurations.Concrete
{
    public class IslemlerMap : IEntityTypeConfiguration<Islemler>
    {
        public void Configure(EntityTypeBuilder<Islemler> builder)
        {
            builder.Property(a => a.isLemSuresi).HasMaxLength(10);
            builder.Property(a => a.isLemUcreti).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(a => a.isLemAdi).IsRequired().HasMaxLength(100);     
            builder.HasOne(a => a.AppUser).WithMany(a => a.Islemlers).HasForeignKey(a => a.AppUserID).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
