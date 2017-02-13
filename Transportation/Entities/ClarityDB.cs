using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Transportation
{
    public class ClarityDB : DbContext, IClarityDB
    {
        [ThreadStatic]
        public static IClarityDB Instance;

        public static void CreateInstance()
        {
            DestroyInstance();
            Instance = new ClarityDB();
        }

        public ClarityDB()
            : base()
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
        }

        public static void DestroyInstance()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }

        public void AddEntity<T>(T entity) where T : class
        {
            this.Set<T>().Add(entity);
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<ColumnAttributeConvention>();
            modelBuilder.Conventions.Remove<ComplexTypeAttributeConvention>();
            modelBuilder.Conventions.Remove<ComplexTypeDiscoveryConvention>();
            modelBuilder.Conventions.Remove<ConcurrencyCheckAttributeConvention>();
            modelBuilder.Conventions.Remove<DatabaseGeneratedAttributeConvention>();
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Remove<ForeignKeyNavigationPropertyAttributeConvention>();
            modelBuilder.Conventions.Remove<ForeignKeyPrimitivePropertyAttributeConvention>();
            modelBuilder.Conventions.Remove<InversePropertyAttributeConvention>();
            modelBuilder.Conventions.Remove<IdKeyDiscoveryConvention>();
            modelBuilder.Conventions.Remove<InversePropertyAttributeConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<SqlCePropertyMaxLengthConvention>();

        }
    }
}
