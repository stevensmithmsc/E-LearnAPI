using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class RequirementConfiguration : EntityTypeConfiguration<Requirement>
    {
        public RequirementConfiguration()
        {
            ToTable("Req", "train");

            HasRequired(r => r.StaffObject)
                .WithMany(p => p.Requirements)
                .HasForeignKey(r => r.Staff);

            HasRequired(r => r.CourseObject)
                .WithMany(c => c.Requirements)
                .HasForeignKey(r => r.Course);

            HasRequired(r => r.StatusObject)
                .WithMany(s => s.Requirements)
                .HasForeignKey(r => r.Status);

            Property(r => r.Comments)
                .HasColumnType("text");
        }
    }
}