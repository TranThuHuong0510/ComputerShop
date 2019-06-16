using Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Mapping
{
    public class BranchMapping : EntityTypeConfiguration<Branch>
    {
        public BranchMapping()
        {
            // Table & Column Mappings
            this.ToTable("Branch");
        }
    }
}
