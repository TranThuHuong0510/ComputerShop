﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Models.Mapping
{
    public class FooterMapping : EntityTypeConfiguration<Footer>
    {
        public FooterMapping()
        {
            // Table & Column Mappings
            this.ToTable("Footers");
        }
    }
}
