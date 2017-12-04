using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            ToTable("Staff", "train");

            Property(p => p.Fname)
                .HasColumnType("varchar")
                .HasMaxLength(35);

            Property(p => p.Sname)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(35);

            Property(p => p.ADAccount)
                .HasColumnType("varchar")
                .HasMaxLength(50);
        }
    }
}