using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host;

namespace Weltenretter.Systeminformationen.Host.Data
{
    public partial class z_Systeminformationen_HostContext : DbContext
    {
        public z_Systeminformationen_HostContext()
        {
        }

        public z_Systeminformationen_HostContext(DbContextOptions<z_Systeminformationen_HostContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> Personen { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.SqlServerOnDeleteConvention));
        }
    }
}