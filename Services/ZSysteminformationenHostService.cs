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


        public async Task ExportPersonenToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/z_systeminformationen_host/personen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/z_systeminformationen_host/personen/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPersonenToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/z_systeminformationen_host/personen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/z_systeminformationen_host/personen/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPersonenRead(ref IQueryable<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> items);

        public async Task<IQueryable<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen>> GetPersonen(Query query = null)
        {
            var items = Context.Personen.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPersonenRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPersonenGet(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);
        partial void OnGetPersonenById(ref IQueryable<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> items);


        public async Task<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> GetPersonenById(int id)
        {
            var items = Context.Personen
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetPersonenById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPersonenGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPersonenCreated(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);
        partial void OnAfterPersonenCreated(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);

        public async Task<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> CreatePersonen(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen personen)
        {
            OnPersonenCreated(personen);

            var existingItem = Context.Personen
                              .Where(i => i.Id == personen.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Personen.Add(personen);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(personen).State = EntityState.Detached;
                throw;
            }

            OnAfterPersonenCreated(personen);

            return personen;
        }

        public async Task<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> CancelPersonenChanges(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPersonenUpdated(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);
        partial void OnAfterPersonenUpdated(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);

        public async Task<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> UpdatePersonen(int id, Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen personen)
        {
            OnPersonenUpdated(personen);

            var itemToUpdate = Context.Personen
                              .Where(i => i.Id == personen.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(personen).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPersonenUpdated(personen);

            return personen;
        }

        partial void OnPersonenDeleted(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);
        partial void OnAfterPersonenDeleted(Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen item);

        public async Task<Weltenretter.Systeminformationen.Host.Models.z_Systeminformationen_Host.Personen> DeletePersonen(int id)
        {
            var itemToDelete = Context.Personen
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPersonenDeleted(itemToDelete);

            Reset();

            Context.Personen.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPersonenDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}