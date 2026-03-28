using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using Weltenretter.Systeminformationen.Host.Data;

namespace Weltenretter.Systeminformationen.Host
{
    public partial class z_Systeminformationen_HostService
    {
        z_Systeminformationen_HostContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly z_Systeminformationen_HostContext context;
        private readonly NavigationManager navigationManager;

        public z_Systeminformationen_HostService(z_Systeminformationen_HostContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }

        /// <summary>
        /// Liefert Personen für die DataGrid-Abfrage.
        /// </summary>
        /// <param name="query">Radzen-Query Parameter.</param>
        /// <returns>Liste der Personen.</returns>
        public async Task<IEnumerable<Models.Person>> GetPersonen(Query query = null)
        {
            IQueryable<Models.Person> items = Context.Personen.AsNoTracking();

            ApplyQuery(ref items, query);

            return await items.ToListAsync();
        }

        /// <summary>
        /// Liefert die Anzahl der Personen für Paging.
        /// </summary>
        /// <param name="query">Radzen-Query Parameter.</param>
        /// <returns>Anzahl der Personen.</returns>
        public async Task<int> GetPersonenCount(Query query = null)
        {
            IQueryable<Models.Person> items = Context.Personen.AsNoTracking();

            ApplyQuery(ref items, query);

            return await items.CountAsync();
        }

        /// <summary>
        /// Lädt eine Person anhand der Id.
        /// </summary>
        /// <param name="id">Personen-Id.</param>
        /// <returns>Person oder null.</returns>
        public async Task<Models.Person> GetPersonById(int id)
        {
            return await Context.Personen.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Erstellt eine neue Person.
        /// </summary>
        /// <param name="person">Neue Person.</param>
        /// <returns>Erstellte Person.</returns>
        public async Task<Models.Person> CreatePerson(Models.Person person)
        {
            Context.Personen.Add(person);
            await Context.SaveChangesAsync();
            return person;
        }

        /// <summary>
        /// Aktualisiert eine bestehende Person.
        /// </summary>
        /// <param name="id">Personen-Id.</param>
        /// <param name="person">Aktualisierte Daten.</param>
        /// <returns>Aktualisierte Person.</returns>
        public async Task<Models.Person> UpdatePerson(int id, Models.Person person)
        {
            var existing = await Context.Personen.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
            {
                throw new InvalidOperationException("Person wurde nicht gefunden.");
            }

            existing.Vorname = person.Vorname;
            existing.Nachname = person.Nachname;

            await Context.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Löscht eine Person.
        /// </summary>
        /// <param name="id">Personen-Id.</param>
        /// <returns>Gelöschte Person.</returns>
        public async Task<Models.Person> DeletePerson(int id)
        {
            var existing = await Context.Personen.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
            {
                throw new InvalidOperationException("Person wurde nicht gefunden.");
            }

            Context.Personen.Remove(existing);
            await Context.SaveChangesAsync();

            return existing;
        }
 
    }
}