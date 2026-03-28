using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Weltenretter.Systeminformationen.Host.Models;

namespace Weltenretter.Systeminformationen.Host.Data
{
    /// <summary>
    /// DbContext des Hostprojekts (Testumgebung).
    /// </summary>
    public partial class z_Systeminformationen_HostContext : DbContext
    {
        /// <summary>
        /// Erstellt eine neue Instanz ohne Optionen.
        /// </summary>
        public z_Systeminformationen_HostContext()
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz mit EF-Core-Optionen.
        /// </summary>
        /// <param name="options">DbContext-Optionen.</param>
        public z_Systeminformationen_HostContext(DbContextOptions<z_Systeminformationen_HostContext> options) : base(options)
        {
        }

        /// <summary>
        /// Test-Entität Personen.
        /// </summary>
        public DbSet<Person> Personen => Set<Person>();

        partial void OnModelBuilding(ModelBuilder builder);

        /// <summary>
        /// Konfiguriert die Modell- und Mappingregeln.
        /// </summary>
        /// <param name="builder">EF ModelBuilder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Tabelle für Testzwecke explizit benennen
            builder.Entity<Person>(entity =>
            {
                entity.ToTable("Personen");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Vorname).IsRequired();
                entity.Property(p => p.Nachname).IsRequired();
            });

            this.OnModelBuilding(builder);
        }

        /// <summary>
        /// Konfiguriert globale Konventionen.
        /// </summary>
        /// <param name="configurationBuilder">Konventions-Builder.</param>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.CascadeDeleteConvention));
            configurationBuilder.Conventions.Remove(typeof(Microsoft.EntityFrameworkCore.Metadata.Conventions.SqlServerOnDeleteConvention));
        }
    }
}
