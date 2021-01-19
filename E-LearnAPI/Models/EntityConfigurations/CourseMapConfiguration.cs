using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class CourseMapConfiguration : EntityTypeConfiguration<CourseMap>
    {
        public CourseMapConfiguration()
        {
            ToTable("CourseModuleMap", "train");

            HasKey(c => c.ID);

            Property(c => c.ModuleName)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("ESRModuleName")
                .HasColumnType("varchar");

            HasRequired(c => c.Course)
                .WithMany(c => c.Modules)
                .HasForeignKey(c => c.CourseID);
        }
    }
}

