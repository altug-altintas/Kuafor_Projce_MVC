using Proje_model.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proje_model.TypeConfigurations.Concrete
{
    public class UlkeMap : IEntityTypeConfiguration<Ulke>
    {
        public void Configure(EntityTypeBuilder<Ulke> builder)
        {
            // Properties
            builder.Property(a => a.ulke_kodu).IsRequired().HasMaxLength(3); 
            builder.Property(a => a.ulke_adi).IsRequired().HasMaxLength(100);

            // Relationships
           
        }
    }
}
