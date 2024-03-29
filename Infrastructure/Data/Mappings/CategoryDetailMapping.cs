﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain;

namespace Infrastructure.Data.Mappings
{
    class CategoryDetailMapping : EntityTypeConfiguration<CategoryDetail>
    {
        public CategoryDetailMapping()
        {
            // PRIMARY KEY

            HasKey(c => c.Id);

            // PROPERTIES

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.Description);

            Property(c => c.Detail);

            Property(c => c.YearStarted);

            Property(c => c.CategoryType);

            HasRequired(c => c.Category)
                .WithMany();

            HasOptional(c => c.PartnerDetail);

        }
    }
}
