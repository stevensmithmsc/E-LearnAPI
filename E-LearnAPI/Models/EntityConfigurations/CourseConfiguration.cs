using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class CourseConfiguration : EntityTypeConfiguration<Course>
    {
        public CourseConfiguration()
        {
            ToTable("Courses", "train");

            Property(c => c.CourseName)
               .IsRequired()
               .HasColumnType("varchar")
               .HasMaxLength(80)
               .HasColumnName("Course");
        }
    }
}