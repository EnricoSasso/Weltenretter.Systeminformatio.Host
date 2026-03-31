using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Weltenretter.Systeminformationen.Host.Data;

namespace Weltenretter.Systeminformationen.Host.Controllers
{
    public partial class Exportz_Systeminformationen_HostController : ExportController
    {
        private readonly z_Systeminformationen_HostContext context;
        private readonly z_Systeminformationen_HostService service;

        public Exportz_Systeminformationen_HostController(z_Systeminformationen_HostContext context, z_Systeminformationen_HostService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/z_Systeminformationen_Host/personen/csv")]
        [HttpGet("/export/z_Systeminformationen_Host/personen/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPersonenToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPersonen(), Request.Query, false), fileName);
        }

        [HttpGet("/export/z_Systeminformationen_Host/personen/excel")]
        [HttpGet("/export/z_Systeminformationen_Host/personen/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPersonenToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPersonen(), Request.Query, false), fileName);
        }
    }
}
