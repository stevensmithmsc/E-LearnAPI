using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class ReportAccessConfiguration : EntityTypeConfiguration<ReportAccess>
    {
        public ReportAccessConfiguration()
        {
            ToTable("Report_Access", "train");

            HasKey(r => r.StaffID);

            Property(r => r.StaffID)
                .HasColumnName("ID");

            HasRequired(r => r.Staff)
                .WithOptional(p => p.ReportAccess);

            Property(r => r.AccessLevel)
                .HasColumnName("ELearn");
                
        }
    }
}