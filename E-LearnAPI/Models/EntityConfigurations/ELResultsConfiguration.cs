using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class ELResultsConfiguration : EntityTypeConfiguration<ELResult>
    {
        public ELResultsConfiguration()
        {
            ToTable("ELearning", "train");

            Property(e => e.PersonName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(e => e.CourseDesc)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(e => e.FromADAcc)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            Property(e => e.Comments)
                .HasColumnType("text");
        }
    }
}