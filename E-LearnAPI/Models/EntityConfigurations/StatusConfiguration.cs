using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class StatusConfiguration : EntityTypeConfiguration<Status>
    {
        public StatusConfiguration()
        {
            ToTable("Statuses", "train");

            Property(s => s.StatusDesc)
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .HasColumnName("Status");
        }
    }
}