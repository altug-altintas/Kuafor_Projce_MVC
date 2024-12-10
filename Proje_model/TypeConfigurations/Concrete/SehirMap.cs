using Proje_model.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.TypeConfigurations.Concrete
{
    public class SehirMap : IEntityTypeConfiguration<Sehir>
    {



        public void Configure(EntityTypeBuilder<Sehir> builder)
        {
            // Properties
            builder.Property(a => a.sehir_plaka).IsRequired().HasMaxLength(2);
            builder.Property(a => a.sehir_adi).IsRequired().HasMaxLength(100);
            builder.Property(a => a.sehir_posta_kod).HasMaxLength(10);

            // Relationships
            builder.HasOne(a => a.Ulke).WithMany(a => a.Sehirler).HasForeignKey(a => a.UlkeId);

        }
    }
}
