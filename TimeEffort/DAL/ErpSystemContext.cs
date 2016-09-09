using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEffort.Models;

namespace TimeEffortCore.DAL
{
    class ErpSystemContext : DbContext
    {
        public ErpSystemContext()
            : base("ErpSystemEntities")
        {
            //this.Database.Connection.Open();
            Database.Log = msg => Trace.Write(msg);
            new DropCreateDatabaseIfModelChanges<ErpSystemContext>();//Database.SetInitializer<ErpSystemContext>(null); // 
        }
        public DbSet<Access> Access { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationType> NotificationType { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Workload> Workload { get; set; }
        public DbSet<WorkloadType> WorkloadType { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("public");
            
            //make generated table names singular
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            /*Enables Table-Per-Type mapping of subclasses superclasses, 
            without this will be mapped as Table-Per-Hierarchy (single table with discriminator attribute)*/
            
            //modelBuilder.Entity<CorporateCustomer>().ToTable("CorporateCustomer");
            //modelBuilder.Entity<IndividualCustomer>().ToTable("IndividualCustomer");

            modelBuilder.Entity<Access>().ToTable("Access", "public");
            modelBuilder.Entity<Customer>().ToTable("Customer", "public");
            modelBuilder.Entity<Notification>().ToTable("Notification", "public");
            modelBuilder.Entity<NotificationType>().ToTable("NotificationType", "public");
            modelBuilder.Entity<Position>().ToTable("Position", "public");
            modelBuilder.Entity<Project>().ToTable("Project", "public");
            modelBuilder.Entity<Role>().ToTable("Role", "public");
            modelBuilder.Entity<UserInfo>().ToTable("UserInfo", "public");
            modelBuilder.Entity<Workload>().ToTable("Workload", "public");
            modelBuilder.Entity<WorkloadType>().ToTable("WorkloadType", "public");


            //modelBuilder.Entity<UserInfo>()
            //        .HasOptional(c => c.UserInfo2)
            //        .WithMany()
            //        .HasForeignKey(c => c.DirectHead);

            base.OnModelCreating(modelBuilder);
        }

       
    }
}
