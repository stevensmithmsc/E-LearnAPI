﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace E_LearnAPI.Models.EntityConfigurations
{
    public class ESRModulesConfiguration : EntityTypeConfiguration<ESRModules>
    {
        public ESRModulesConfiguration()
        {
            ToTable("ESRModules", "train");

            HasKey(e => e.ID);

            HasOptional(e => e.Staff)
                .WithMany()
                .HasForeignKey(e => e.StaffID);

            Property(e => e.ModuleName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);

            HasOptional(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseID);

            Property(e => e.Processed)
                .IsRequired();

            Property(e => e.Comments)
                .HasColumnType("text");
        }
    }
}