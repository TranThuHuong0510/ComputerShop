using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Models.Mapping;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        private static IReadOnlyDictionary<Type, IReadOnlyCollection<PropertyInfo>> _ignoredProperties;

        static DatabaseContext()
        {
            Database.SetInitializer<DatabaseContext>(null);
        }
        public DatabaseContext()
            : base("Name=DatabaseContext")
        {
        }
        public static IReadOnlyDictionary<Type, IReadOnlyCollection<PropertyInfo>> IgnoredProperties
        {
            get
            {
                return _ignoredProperties;
            }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //TODO Mapping
            modelBuilder.Configurations.Add(new CompanyMapping());
            modelBuilder.Configurations.Add(new StorerMapping());
            modelBuilder.Configurations.Add(new BranchMapping());

            base.OnModelCreating(modelBuilder);
        }
    }
}
